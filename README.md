# [Auxquimia](https://jira.izertis.com/browse/Auxquimia)

## Introducción

La aplicación sera utilizada para la automatizacion del proceso productivo de Auxquimia. Dicha aplicación deberá comunicarse con un OPC que será el encargado de generar las formulas de las recetas para generar el producto final.
El proyecto será para ejecutarse tanto en plantas fijas como plantas moviles. para estas plantas móviles en lugar de comunicarse con el OPC tendrán la receta en la BBDD y será propia, estas estaciones móviles se deberán de comunicar
con nuestro proyecto que sera el encargado de mantener todo el flujo y ordenes de trabajo. para esta comunicación utilizaremos colas tipo Kafka de modo que asegure una comunicación y o suframos de pérdidas de datos.

### Documentación adicional

Por ahora se esta trabajando en el análisis pero una vez finalizado se alojará en Alfresco

### Proyectos relacionados

Auxquimia-front --> frontal para la aplicación
Auxquimia-satellite --> proyecto encargado de la comunicación entre las estaciones (plantas) moviles y la central

## Quickstart

### Dependencias

A continuación se identifican las principales dependencias de la aplicación:

* Gestion de colas -> https://github.com/rebus-org/Rebus

### Compilación

Será necesario que pase los tests para compilar

### Ejecución

La aplicación se arranca lanzando el proyecto desde el propio IDE (Ejemplo: Visual Studio)

### Developer quickstart

[Guía de inicio rápido para desarrolladores](docs/dev-quickstart.md)

## Diseño Técnico

Se trata de un proyecto con una parte frontal desarrollada en angular y otra solución desarrollada en .net core con una arquitectura de n-capas en la que habrá una capa de controlador otra de servicios y otra de repositorios.
Además de esto, se incluye un proyecto para la gestión de las colas y asi comunicar las estaciones móviles con la central

### Tecnología

* Plataforma empleada: net core
* Tipo de Proyecto: Servicio web
* Version del Framework utilizada: net core 2.1
* Plataforma de despliegue: Linux con Docker

### Diagrama de Arquitectura

El siguiente diagrama ilustra la arquitectura de alto nivel entre la aplicación y el resto de servicios de su ecosistema:

[Arquitectura](docs/imgs/hl_arch_diagram.png "Arquitectura")

### Estructura del proyecto

* *Auxquimia*: Capa de intercambio de información entre el front y el back.
* *Auxquimia.Service*: Modelo de dominio de la aplicación, implementación de capa de servicios y acceso a datos.
* *Auxquimia.Batch*: Módulo de la aplicación para realizar el lanzamiento de tareas programadas.

### Integración con otros Sistemas

La aplicación se integra con los siguientes sistemas externos a sí misma:

* *SQL Server*: El acceso a SQL Server se realiza a través del driver SqlClientDriver utlizando las credenciales provistas a tal efecto.
* *NetSuite (ERP)*: La plataforma se conecta con el ERP mediante protocolos OPC de modo que se puedan recuperar las ordenes de trabajo así como las recetas, además, se iran actualizando los estados de las órdenes de trabajo.
* *Rebus (para colas tipo Kafka)*: Debemos conectarnos a la aplicación de Auxquimia desde las plantas moviles enviando y escribiendo los datos necesarios para la gestion de las órdenes de trabajo.