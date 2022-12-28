## Herramientas
* Será necesario tener instalado Visual Studio 2017 o superior
* *Instalar Microsoft .NET core sdk 2.1 o superior: https://dotnet.microsoft.com/download/dotnet-core/2.1
* Instalar Docker

### Configurar y arrancar docker
	
	1. ir al proyecto carpeta/docker-devenv
	2. abrir una terminal de Windows
	3. lanzar el comando docker-compose-up
	
### Configurar proyecto para arrancarlo desde Visual Studio

	1. fijar la variable de entorno ASPNETCORE_ENVIRONMENT a "local". Para ello acudir a las propiedades del proyecto web
	2. en la pantalla de depurar setearemos la url al valor que deseemos para despues conectar el front (ejemplo: http://localhost:5000 )
	
### Arrancar el proyecto

	1. Una vez realizados los pasos anteriores abir el gestor de BBDD instalado y crear la BBDD requerida para el proyecto si no existiese previamente
	2. Lanazar el proyecto.
