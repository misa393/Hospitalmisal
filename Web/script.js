// Usa SIEMPRE la URL correcta de tu API (Azure o localhost activo)
const API_URL = "https://localhost:51463/api/Pacientesmisael";
// O si ya publicaste en Azure:


const medicosValidos = ["MED-1010", "MED-2020", "MED-3030", "MED-4040", "MED-5050"];

document.getElementById("tabPacientes").addEventListener("click", () => {
    document.getElementById("pacientesPanel").style.display = "block";
    document.getElementById("registroPanel").style.display = "none";
    cargarPacientes();
});

document.getElementById("tabRegistro").addEventListener("click", () => {
    document.getElementById("pacientesPanel").style.display = "none";
    document.getElementById("registroPanel").style.display = "block";
});

async function cargarPacientes() {
    try {
        const res = await fetch(API_URL);
        if (!res.ok) throw new Error("Error al cargar pacientes");
        const pacientes = await res.json();

        const tbody = document.getElementById("pacientesTable");
        tbody.innerHTML = "";

        pacientes.forEach(p => {
            const row = document.createElement("tr");
            if (p.nivelGravedad === 5) row.style.backgroundColor = "#f8d7da";
            row.innerHTML = `
        
        <td>${p.medicoResponsable}</td>
        <td>${p.nivelGravedad}</td>
        <td>${p.estado}</td>
        <td>${p.fechaIngreso}</td>`;
            tbody.appendChild(row);
        });
    } catch (err) {
        alert("No se pudo conectar con la API (¿está corriendo?)");
    }
}

document.getElementById("registroForm").addEventListener("submit", async (e) => {
    e.preventDefault();

    const medico = document.getElementById("medicoResponsable").value.trim();
    if (!medicosValidos.includes(medico)) {
        document.getElementById("mensajeError").innerText = "Acceso Denegado: Carnet inválido";
        return;
    }

    const nuevoPaciente = {
        idPaciente: "PAC-" + Date.now(), // requerido por tu backend
        medicoResponsable: medico,
        nivelGravedad: parseInt(document.getElementById("nivelGravedad").value),
        estado: document.getElementById("estado").value,
        fechaIngreso: new Date(document.getElementById("fechaIngreso").value).toISOString()
    };

    try {
        const res = await fetch(API_URL, {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: JSON.stringify(nuevoPaciente)
        });

        if (res.status === 401) {
            document.getElementById("mensajeError").innerText = "Acceso Denegado: Carnet inválido";
            return;
        }

        if (!res.ok) {
            let text = await res.text();
            document.getElementById("mensajeError").innerText = "Error al guardar: " + text;
            return;
        }

        alert("Paciente registrado correctamente");
        cargarPacientes();
    } catch (err) {
        document.getElementById("mensajeError").innerText = "Error en el registro (API no responde)";
    }
});
