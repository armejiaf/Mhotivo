Mhotivo
=======

Se requiere SQLServer 2012
El IDE recomendado es "Visual Studio 2013"
	-de usar otro, se requiere que este soporte desarrolo en .NET

Lo siguiente es crear una base de datos en SQLServer llamada "Mhotivo", esta se usara por defecto
	-De tener otra version, se debera cambiar el valor del "connections tring" en los archivos "web.config" de los proyectos "Mhotivo" y "Mhotivo.ParentSite" con el valor de la base de datos creada.

CORRIENDOLO POR PRIMERA VEZ
---------------------------
La primera vez que se corra el proyecto "Mhotivo" el seeder llenara la base de datos con valores por defecto, esto causa que se tarde unos cuantos segunudos
	-el proyecto "Mhotivo" contiene la parte administrativa (cuentas admin y maestros)

El proyecto "Mhotivo.ParentSite" es el login de los padres

LOGIN
-----
Una de las cuentas creadas es la de "admin@mhotivo.org" y su clave es "password", esto se puede utilizar para ingresar a la pagina de administracion
Para ingresar a la pagina de maestros, se usa "teacher@mhotivo.org" y su clave es "password"
Para la pagina de padres, "parent@mhotivo.org" y su clave es "password"

Tambien se crea un padre generico, un maestro generico y todos los grados, todo esto es creado para peuebas.

