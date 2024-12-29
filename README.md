Progetto: Script per Copia di File da Progetto C#

Descrizione

Questo script, scritto in C#, consente di copiare tutti i file presenti in specifiche cartelle di un progetto C# in una nuova cartella situata sul desktop, senza mantenere la struttura delle sottocartelle.

Cartelle Analizzate

I file vengono cercati e copiati dalle seguenti cartelle del progetto:

Controller

Data

DTOs

Models

Profiles

Service

I file raccolti saranno tutti collocati nella cartella di destinazione senza suddivisione in sottocartelle. In caso di conflitti di nomi (file con lo stesso nome), lo script rinominerà automaticamente i file aggiungendo un identificatore univoco.
