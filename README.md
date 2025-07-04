# CludeMedSync

📘 Regras obrigatórias
Na lógica do agendamento (não só no CRUD), você precisa validar:

🔄 Um paciente não pode ter mais de uma consulta com o mesmo profissional no mesmo dia.

🚫 Um profissional não pode atender mais de uma pessoa no mesmo horário.

🕗 Consultas só podem ser entre 08:00 e 18:00, de segunda a sexta.

⏱️ Cada consulta dura 30 minutos.

✅ Agendamento só pode ocorrer se houver disponibilidade real.