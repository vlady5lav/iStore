# Online Store

## Store Url

[http://www.alevelwebsite.com](http://www.alevelwebsite.com)

## Swagger Urls

[http://www.alevelwebsite.com:5000/swagger/index.html](http://www.alevelwebsite.com:5000/swagger/index.html)

[http://docker.host.internal:5000/swagger/index.html](http://docker.host.internal:5000/swagger/index.html)

[http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)

## Update hosts file

### For Windows

#### Enter the following commands in the Command Line (cmd.exe) with Administrative Privileges

#### Backup your original hosts

```
copy /V %WINDIR%\System32\drivers\etc\hosts %WINDIR%\System32\drivers\etc\hosts.original.bak
```

#### Modify hosts file

```
echo 0.0.0.0 www.alevelwebsite.com >> %WINDIR%\System32\drivers\etc\hosts
```

```
echo 127.0.0.1 www.alevelwebsite.com >> %WINDIR%\System32\drivers\etc\hosts
```

```
echo 192.168.0.1 www.alevelwebsite.com >> %WINDIR%\System32\drivers\etc\hosts
```

### For Mac or Linux

#### Enter the following commands in the Terminal

#### Backup your original hosts

```
sudo cp /etc/hosts /etc/hosts.alevel.bak
```

#### Modify hosts file

```
sudo -- sh -c -e "echo '0.0.0.0 www.alevelwebsite.com' >> /etc/hosts";
```

```
sudo -- sh -c -e "echo '127.0.0.1 www.alevelwebsite.com' >> /etc/hosts";
```

```
sudo -- sh -c -e "echo '192.168.0.1 www.alevelwebsite.com' >> /etc/hosts";
```

## Build apps and run Docker container

#### Enter the following commands in the Terminal or in the Command Line (cmd.exe)

#### Development

##### Make a clean Build and Up

```
docker compose -f docker-compose.yml -f docker-compose.override.yml build --no-cache --pull
```

```
docker compose -f docker-compose.yml -f docker-compose.override.yml up --force-recreate --pull missing --remove-orphans --renew-anon-volumes -d
```

##### Just Build and Up

```
docker compose -f docker-compose.yml -f docker-compose.override.yml up --pull missing --remove-orphans -d
```

#### Production

##### Make a clean Build and Up

```
docker compose -f docker-compose.yml -f docker-compose.prod.yml build --no-cache --pull
```

```
docker compose -f docker-compose.yml -f docker-compose.prod.yml up --force-recreate --pull missing --remove-orphans --renew-anon-volumes -d
```

##### Just Build and Up

```
docker compose -f docker-compose.yml -f docker-compose.prod.yml up --pull missing --remove-orphans -d
```

## Migration tips

#### List Migrations

```
dotnet ef --startup-project Catalog/Catalog.Host migrations list --project Catalog/Catalog.Host
```

#### Add Migration

```
dotnet ef --startup-project Catalog/Catalog.Host migrations add InitialMigration --project Catalog/Catalog.Host
```

#### Update Migration

```
dotnet ef --startup-project Catalog/Catalog.Host database update InitialMigration --project Catalog/Catalog.Host
```

#### Remove Migration

```
dotnet ef --startup-project Catalog/Catalog.Host migrations remove --project Catalog/Catalog.Host -f
```
