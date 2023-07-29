# Katministratie

Het was de bedoellingg de katministratie eerst op Azure te laten draaien maar daar waren de kosten niet duidelijk genoeg. Dus hier wordt beschreven hoe de lokale webserver voor de katministratie wordt opgezet. De info is gehaald uit.

In eerste instantie getest met een Ubuntu server die in een VM draait

## Voorbereiden server
- Download de ubuntu server ISO
- Creeer een nieuwe VM die de ISO gebruikt voor installatie
- Zorg ervoor dat je de ssh-server ook installeerd

## dotNet runtime installeren
Installeren van de dotnet runtime zodat een ASP.NET applicatie kan draaien, voer de volgende commando's uit: 
- ?? sudo wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
- ?? ./dotnet-install.sh
- sudo apt-get update
- sudo apt-get install -y apt-transport-https
- sudo apt-get update
- sudo apt-get install -y aspnetcore-runtime-6.0

## Webserver installeren
voer het volgende commando uit:
- sudo apt-get install apache2

## Apache2 configureren
voer de volgende commando's uit:
- sudo a2enmod rewrite
- sudo a2enmod proxy
- sudo a2enmod proxy_http
- sudo a2enmod headers
- sudo a2enmod ssl
- sudo service apache2 restart

## dotNet applicatie configureren
Als de dotNet MAUI applicatie draait dan start deze een eigen webserver op. Dit wordt de kestrel web server genoemd en draaid op localhost en poort 5000.
Maak een **sample.service** file aan met onderstaande inhoud en plaats deze in **/etc/systemd/system**:
```
[Unit]
Description=Running ASP.NET Core on Ubuntu 18.04 Webserver APACHE
[Service]
WorkingDirectory=/var/www/sample/
ExecStart=/usr/bin/dotnet /var/www/sample/sample.dll
Restart=always
# Restart service after 10 seconds if the dotnet service crashes
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=dotnet-example
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false
[Install]
WantedBy=multi-user.target
```
In bovenstaande configuratiefile zijn de parameters **ExecStart** en **WorkingDirectory** van belang. Die moeten goed staan. Zoals je ziet zal je de dll van je applicatie moeten invullen als ook de map aar je de published files naartoe hebt gekopieerd. In MSVS zal je op het project publish moeten kiezen en publish dan naar een folder. Deze geproduceerde files zal je via een ftp client moeten uploaden naar in dit voorbeeld /var/www/html/sample. Als je dit hebt gedaan voer dan de volgende twee opdrachten uit:

- sudo systemctl enable sample.service
- sudo systemctl start sample.service

met het **px ax** commando kan je kijken of er een dotnet process is die de dll aan het uitvoeren is.

## Apache2 configureren als proxy
Maak een file sample.conf aan die je in **/etc/apache2/sites-available** plaats. Hiermee geef je aan apache2 door dat de / routeert naar de kestrel server die op de localhost draait.
```
<VirtualHost *:80>
ProxyPass "/" "http://127.0.0.1:5000/"
ProxyPassReverse "/" "http://127.0.0.1:5000/"
ErrorLog ${APACHE_LOG_DIR}/sample-error.log
</VirtualHost>
```
Logging kan je vinden in /var/log/aapche2
De proxy zet je aan met de volgende commando's
- sudo a2ensite sample.conf
- sudo apachectl configtest

## Installeren mysql server
- sudo apt install mysql-server
- sudo systemctl start mysql.service
- sudo mysql
- mysql> ALTER USER 'root'@'localhost' IDENTIFIED WITH mysql_native_password BY '<PASSWORD>';
- mysql> exit
- mysql -u root -p
- mysql> ALTER USER 'root'@'localhost' IDENTIFIED WITH auth_socket;
- mysql> exit

Controleer de installatie met systemctl **status mysql.service**

>[ref: 1](https://www.syncfusion.com/blogs/post/hosting-multiple-asp-net-core-apps-in-ubuntu-linux-server-using-apache.aspx)

>[ref: 2](https://www.digitalocean.com/community/tutorials/how-to-install-mysql-on-ubuntu-20-04)
