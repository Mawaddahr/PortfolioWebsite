# Hosting op Azure

De site draait in de resource group `rg-mawadda` (West Europe):

| Onderdeel | Azure-resource | Adres |
|---|---|---|
| Publieke site (`UI/`) | Static Web App `swa-mawadda-ui` (Free) | https://www.mawadda.nl |
| Admin-site (`UIAdmin/`) | Static Web App `swa-mawadda-admin` (Free) | https://admin.mawadda.nl |
| API (root, .NET) | Container App `ca-mawadda-api` (0–1 replica) | https://api.mawadda.nl |
| Database | PostgreSQL Flexible Server, Burstable B1ms | — |
| Geüploade afbeeldingen | Storage account, container `images` | — |

Alle infrastructuur staat in [`infra/main.bicep`](infra/main.bicep).

## Workflows

- **Deploy naar Azure** (`.github/workflows/deploy.yml`): draait bij elke push naar `master`. Bouwt de API-image (GitHub Container Registry), rolt de infrastructuur uit en zet beide frontends online. Het overzicht van de run bevat de tijdelijke adressen en de DNS-records.
- **Domeinen koppelen** (`.github/workflows/custom-domains.yml`): handmatig starten nadat de DNS-records staan.
- **Build-controle** (`.github/workflows/ci.yml`): controleert bij elke pull request of alles nog bouwt.

## Benodigde instellingen in GitHub

Onder *Settings → Secrets and variables → Actions*:

| Naam | Soort | Inhoud |
|---|---|---|
| `AZURE_CLIENT_ID` | secret | App-ID van `github-mawadda-deploy` |
| `AZURE_TENANT_ID` | secret | Tenant-ID |
| `AZURE_SUBSCRIPTION_ID` | secret | Abonnements-ID |
| `POSTGRES_ADMIN_PASSWORD` | secret | Databasewachtwoord (geen `;` of aanhalingstekens) |
| `ADMIN_EMAILS` | variable | Google-adres(sen) die content mogen wijzigen, komma-gescheiden |
| `POSTGRES_LOCATION` | variable (optioneel) | Andere regio voor de database, bijv. `northeurope` |

## Eerste keer

1. Merge naar `master`. De eerste run stopt met de melding dat de API-image privé is: zet het package `portfoliowebsite-api` op **Public** (link staat in de foutmelding) en klik op *Re-run failed jobs*.
2. Zet de DNS-records uit het overzicht van de run bij je DNS-provider.
3. Voeg in Firebase (*Authentication → Settings → Authorized domains*) het tijdelijke admin-adres en `admin.mawadda.nl` toe.
4. Start **Domeinen koppelen**, en daarna nog één keer **Deploy naar Azure**.

## Lokaal ontwikkelen

Lokaal werkt alles zoals voorheen (`run-dev.bat` of `docker-compose`). Zonder `Storage__ConnectionString` gaan uploads naar `wwwroot/imguploads`. Zonder `Auth__AdminEmails` mag lokaal (Development) iedere ingelogde Firebase-gebruiker wijzigen. Inloggen op de admin-site is wel nodig, omdat de API een token verwacht.

De "About me"-tekst staat nu in de database (tabel `AboutMe`) in plaats van in `Database/AbtMeText.json`. Bij een lege database wordt hij eenmalig uit dat bestand gevuld.
