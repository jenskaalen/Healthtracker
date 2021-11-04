# Healthtracker

How to set up

1. create integrationconfig.json and include in project Healthtracker.Web

```
{
  "GoogleClientId": "123123123123-5g5feferr7q6fn241masdasdased5vm8gg4q2.apps.googleusercontent.com",
  "GoogleClientSecret": "verysecretId",
  //not necessary unless you will use fitbit integration
  "FitbitClientId": "22fitbitID",
  "FitbitClientBase64": "clientBase64",
  //ravendb certificate
  "CertificateName": "certi.pfx",
  "CertificatePassword": "feaeae",
  "RavenDbUrl": "http://ravendb:8080",
  "RavenDbDatabaseName": "LogDb"
}
```

2. Open Healthtracker.Web\wwwroot folder in cmd and run "npm start" 
