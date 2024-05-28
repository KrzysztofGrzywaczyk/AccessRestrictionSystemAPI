# Access Control System API

## .NET, Azure SQL DataBase,  API-Key Authentication, MediatR, Entity Framework

This project is an API for universal **Access Control System**. It is a complete access control solution that enables access restrictions management.</br>
Based on the following assumptions:</br>
- System is using devices that control access via access cards.</br>
- It consists of an arbitrary number of devices.</br>
- Each device has 8 slots (doors, gates, access stations, etc.) 
- All slots slots of the device can be associated withspecific access cards and vice versa  by assigning access permissions."

The API is based on the mediator pattern.</br>
Modyfing and creating new entities is implemented using PUT requests for create or update functionality.
Projet contains full Swagger documentation for endpoints, data schemas, parameters etc.

**The project is covered by both integration and unit tests.**

### API delivers the following functionality :**</br>
(more detailed documentation available via **/swagger**)

**Endpoints for Access Cards managment are:**</br>
>**GET**
/accesscards</br>
>**PUT**
/accesscards/</br>
>**DELETE**
/accesscards</br>

GET and DELETE Access Cards may be filtered by passing card values as array of strings.</br>
PUT Access Cards is realized by passing card values in request body.

**Endpoints for Slots managment are:**</br>
>**GET**
accesscontroldevices/{deviceId}/slots</br>
>**PUT**
accesscontroldevices/{deviceId}/slots
>**DELETE**
accesscontroldevices/{deviceId}/slots</br>

GET and DELETE Slots: </br>
' require to pass deviceId of slot's device in path</br>
' may be filtered by passing unique slot name or slot number (cannot be passed simultaneously)</br>
PUT Slot is realized by passing list of slots (slots name and his 1-8 number on device) in request body and deviceId of slot's device in path.


**Endpoints for Access Cards managment are:**</br>
>**GET**
/accesscards</br>
>**PUT**
/accesscards/</br>
>**DELETE**
/accesscards</br>

GET and DELETE accesses may be filtered by passing access card values as an array of strings in query.</br>
PUT accesses is realized by passing list of card values in request body.</br>

**Endpoints for devices existing in the system managment are:**</br>
>**GET**
/devices</br>
>**PUT**
/devices/{deviceId}</br>
>**DELETE**
/devices/{deviceId}</br>

GET devices may be filtered by passing deviceId values as an array of strings in query.</br>
DELETE device require device Id in path.</br>
PUT device require device Id in path and deviceName in request body.

</br>

### Access Managment - the main part of the system - can be done in two ways" :**</br>

**Endpoints for assigning access cards to slots:**</br>
>**GET**
/devices/{deviceId}/accesses</br>
>**PUT**
/devices/{deviceId}/accesses</br>
>**DELETE**
/devices/{deviceId}/accesses</br>
>**PUT**
/devices/{sourceDeviceId}/copyaccessesto/{targetDeviceId}</br>
>**PUT**
/devices/{sourceDeviceId}/moveaccessesto/{targetDeviceId}</br>

GET and DELETE accesses: </br>
' require to pass deviceId of slot's device in path</br>
' may be filtered by passing unique slot name or slot number (cannot be passed simultaneously)</br>
' delete may be also realised by passing access cards values in query</br>
PUT accesses is realized by passing list of slots (slots name and his 1-8 number on device) in request body and deviceId of slot's device in path.</br>
To those slots will be assigned access cards with values passed in request body as list of strings.</br>
COPY and MOVE is moving all accesses from one devuce to another</br>
If on target device do not exists slots indicated on the source device - they are created.</br>
COPY method leaves source accesses and duplicate them on target device.</br>
MOVE metgod duplicates accesses on targer device and thene delete them from the souce device.

**Endpoints for assigning slots to particular access cards:**</br>
>**GET**
/accesscards/{accessCardValue}/accesses</br>
>**PUT**
/accesscards/{accessCardValue}/accesses</br>
>**DELETE**
/accesscards/{accessCardValue}/accesses</br>

GET and DELETE accesses of an accaess card is realized by passing in path value of access card and may be filtered by passing in query: deviceId, slot names or slot numbers.</br>
PUT accesses of an access card is realized by passing in path access card that should have accesses assigned and a list of slots (represented as a list of objects: deviceId + slot name or deviceId + slot number) in request body.

