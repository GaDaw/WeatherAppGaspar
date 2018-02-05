# WeatherAppGaspar

Descripción: 

Aplicación web realizada con Asp .Net Core , MSSQL, y OpenWeather para proveer datos a una aplicaciones metereológica.


Contenido:

 - Modelo de usuarios con gestión CRUD y vistas.[Modelo: ID, Username, Password, FullName, Dni, Latitud, Longitud, Metereology y Forecast]
        
    - Contraseñas Hasheadas
    - Obtención de coordenadas de usuario al crearse
    - Obtención de información metereológica actual al crearse
    - Se crea una url a una API propia (que consulta datos a Openweather) con el forecast de su ubicación para las próximas horas
        
 - API para consultar Información metereológica y forecast para los usuarios creados
 
 Comenzando:
 
 Este proyecto fue desarrollado y probado de manera local. Se puede testear con Microsoft Visual Studio y IIS Express.
 Tenga especial cuidad con el puerto en su Localhost ya que puede varias respecto al del proyecto.
 
 
 
Autor:

Gaspar Cascallana Prol
