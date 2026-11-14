// Infrastructuur voor mawadda.nl, uitgerold in één resource group.
// Wordt aangeroepen door .github/workflows/deploy.yml.
//
// Kostenplafonds die hier bewust zijn ingebouwd:
// - API (Container App): schaalt naar 0, maximaal 1 replica, 0,25 vCPU / 0,5 GB
// - PostgreSQL: vaste Burstable B1ms, 32 GB, geen automatische groei, geen HA, geen geo-backup
// - Static Web Apps: Free-tier
// - Storage: Standard LRS
// - Log Analytics: 30 dagen bewaren, max. 0,1 GB per dag

targetScope = 'resourceGroup'

@description('Regio voor de meeste resources.')
param location string = resourceGroup().location

@description('Regio voor PostgreSQL. Wijk uit naar bijv. northeurope of swedencentral als West Europe geen capaciteit heeft voor jouw abonnement.')
param postgresLocation string = location

@description('Regio voor de Static Web Apps (beschikbaar o.a. in westeurope).')
param staticWebAppLocation string = 'westeurope'

@description('Korte naam die in alle resourcenamen terugkomt.')
param prefix string = 'mawadda'

@description('Het eigen domein.')
param domain string = 'mawadda.nl'

@description('Volledige naam van de API-image, bijv. ghcr.io/mawaddahr/portfoliowebsite-api:<sha>.')
param apiImage string

@description('Gebruikersnaam van de PostgreSQL-beheerder.')
param postgresAdminUser string = 'mawaddaadmin'

@secure()
@description('Wachtwoord van de PostgreSQL-beheerder (uit GitHub-secret POSTGRES_ADMIN_PASSWORD).')
param postgresAdminPassword string

@description('Firebase-project waarmee de admin inlogt.')
param firebaseProjectId string = 'portfoliowebsite-admin-7f818'

@description('Komma-gescheiden e-mailadressen die content mogen wijzigen.')
param adminEmails string

@description('Eigen domein voor de API (bijv. api.mawadda.nl). Leeg tot het domein gekoppeld is.')
param apiCustomDomain string = ''

@description('ID van het beheerde certificaat voor apiCustomDomain. De workflow zoekt dit zelf op, zodat een nieuwe deploy de domeinkoppeling niet weghaalt.')
param apiCertificateId string = ''

var suffix = uniqueString(resourceGroup().id)
var databaseName = 'portfolio'
var apiAppName = 'ca-${prefix}-api'

// ---------- Logging ----------

resource logs 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: 'log-${prefix}'
  location: location
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
    workspaceCapping: {
      dailyQuotaGb: json('0.1')
    }
  }
}

// ---------- Opslag voor geüploade afbeeldingen ----------

resource storage 'Microsoft.Storage/storageAccounts@2023-05-01' = {
  name: 'st${prefix}${suffix}'
  location: location
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    accessTier: 'Hot'
    allowBlobPublicAccess: true // afbeeldingen moeten publiek leesbaar zijn voor de site
    allowSharedKeyAccess: true
    minimumTlsVersion: 'TLS1_2'
    supportsHttpsTrafficOnly: true
  }
}

resource blobService 'Microsoft.Storage/storageAccounts/blobServices@2023-05-01' = {
  parent: storage
  name: 'default'
}

resource imagesContainer 'Microsoft.Storage/storageAccounts/blobServices/containers@2023-05-01' = {
  parent: blobService
  name: 'images'
  properties: {
    publicAccess: 'Blob' // alleen losse bestanden leesbaar, geen lijst van de inhoud
  }
}

// ---------- Database ----------

resource postgres 'Microsoft.DBforPostgreSQL/flexibleServers@2024-08-01' = {
  name: 'psql-${prefix}-${suffix}'
  location: postgresLocation
  sku: {
    name: 'Standard_B1ms'
    tier: 'Burstable'
  }
  properties: {
    version: '16'
    administratorLogin: postgresAdminUser
    administratorLoginPassword: postgresAdminPassword
    storage: {
      storageSizeGB: 32
      autoGrow: 'Disabled'
    }
    backup: {
      backupRetentionDays: 7
      geoRedundantBackup: 'Disabled'
    }
    highAvailability: {
      mode: 'Disabled'
    }
    network: {
      publicNetworkAccess: 'Enabled'
    }
    authConfig: {
      activeDirectoryAuth: 'Disabled'
      passwordAuth: 'Enabled'
    }
  }
}

resource postgresDatabase 'Microsoft.DBforPostgreSQL/flexibleServers/databases@2024-08-01' = {
  parent: postgres
  name: databaseName
  properties: {
    charset: 'UTF8'
    collation: 'en_US.utf8'
  }
}

// Laat Azure-diensten (zoals de Container App) verbinden. Toegang blijft
// beschermd met gebruikersnaam/wachtwoord en TLS. Een privénetwerk kost extra.
resource postgresAllowAzure 'Microsoft.DBforPostgreSQL/flexibleServers/firewallRules@2024-08-01' = {
  parent: postgres
  name: 'AllowAzureServices'
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

// ---------- Frontends ----------

resource siteUi 'Microsoft.Web/staticSites@2023-12-01' = {
  name: 'swa-${prefix}-ui'
  location: staticWebAppLocation
  sku: {
    name: 'Free'
    tier: 'Free'
  }
  properties: {}
}

resource siteAdmin 'Microsoft.Web/staticSites@2023-12-01' = {
  name: 'swa-${prefix}-admin'
  location: staticWebAppLocation
  sku: {
    name: 'Free'
    tier: 'Free'
  }
  properties: {}
}

// ---------- API ----------

resource containerEnv 'Microsoft.App/managedEnvironments@2024-03-01' = {
  name: 'cae-${prefix}'
  location: location
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logs.properties.customerId
        sharedKey: logs.listKeys().primarySharedKey
      }
    }
  }
}

var apiDefaultUrl = 'https://${apiAppName}.${containerEnv.properties.defaultDomain}'

var dbConnectionString = 'Host=${postgres.properties.fullyQualifiedDomainName};Database=${databaseName};Username=${postgresAdminUser};Password=${postgresAdminPassword};SSL Mode=Require;Maximum Pool Size=20'

var blobConnectionString = 'DefaultEndpointsProtocol=https;AccountName=${storage.name};AccountKey=${storage.listKeys().keys[0].value};EndpointSuffix=${environment().suffixes.storage}'

var baseEnv = [
  { name: 'ASPNETCORE_ENVIRONMENT', value: 'Production' }
  { name: 'ASPNETCORE_FORWARDEDHEADERS_ENABLED', value: 'true' }
  { name: 'ConnectionStrings__ConnectionString', secretRef: 'db-connection' }
  { name: 'Storage__ConnectionString', secretRef: 'blob-connection' }
  { name: 'Storage__Container', value: imagesContainer.name }
  { name: 'Auth__FirebaseProjectId', value: firebaseProjectId }
  { name: 'Auth__AdminEmails', value: adminEmails }
  { name: 'PublicApiUrl', value: apiDefaultUrl }
]

// Toegestane frontends voor CORS (eigen domein + de tijdelijke Azure-adressen)
var corsEnv = [
  { name: 'Cors__AllowedOrigins__0', value: 'https://${domain}' }
  { name: 'Cors__AllowedOrigins__1', value: 'https://www.${domain}' }
  { name: 'Cors__AllowedOrigins__2', value: 'https://admin.${domain}' }
  { name: 'Cors__AllowedOrigins__3', value: 'https://${siteUi.properties.defaultHostname}' }
  { name: 'Cors__AllowedOrigins__4', value: 'https://${siteAdmin.properties.defaultHostname}' }
]

resource api 'Microsoft.App/containerApps@2024-03-01' = {
  name: apiAppName
  location: location
  properties: {
    managedEnvironmentId: containerEnv.id
    configuration: {
      activeRevisionsMode: 'Single'
      ingress: {
        external: true
        targetPort: 8080
        transport: 'auto'
        allowInsecure: false
        customDomains: empty(apiCertificateId) || empty(apiCustomDomain) ? [] : [
          {
            name: apiCustomDomain
            certificateId: apiCertificateId
            bindingType: 'SniEnabled'
          }
        ]
      }
      secrets: [
        { name: 'db-connection', value: dbConnectionString }
        { name: 'blob-connection', value: blobConnectionString }
      ]
    }
    template: {
      containers: [
        {
          name: 'api'
          image: apiImage
          resources: {
            cpu: json('0.25')
            memory: '0.5Gi'
          }
          env: concat(baseEnv, corsEnv)
        }
      ]
      scale: {
        minReplicas: 0 // geen verkeer = geen kosten
        maxReplicas: 1 // harde bovengrens, ook bij een piek of aanval
        rules: [
          {
            name: 'http'
            http: {
              metadata: {
                concurrentRequests: '50'
              }
            }
          }
        ]
      }
    }
  }
  dependsOn: [
    postgresDatabase
    postgresAllowAzure
  ]
}

// ---------- Uitvoer voor de workflow en de DNS-instellingen ----------

output apiUrl string = apiDefaultUrl
output apiFqdn string = api.properties.configuration.ingress.fqdn
output apiName string = api.name
output environmentName string = containerEnv.name
output customDomainVerificationId string = api.properties.customDomainVerificationId
output uiSiteName string = siteUi.name
output uiHostname string = siteUi.properties.defaultHostname
output adminSiteName string = siteAdmin.name
output adminHostname string = siteAdmin.properties.defaultHostname
