import { auth } from './firebaseConfig.jsx';

// Adres van de API. Bij het bouwen in GitHub Actions wordt VITE_API_URL ingevuld;
// lokaal valt hij terug op de .NET-backend op poort 5142.
export const API_URL = (import.meta.env.VITE_API_URL || 'http://localhost:5142').replace(/\/$/, '');

// Firebase ID-token van de ingelogde admin, zodat de API kan controleren wie er wijzigt.
export async function getAuthHeaders() {
    const user = auth.currentUser;
    if (!user) return {};
    const token = await user.getIdToken();
    return { Authorization: `Bearer ${token}` };
}
