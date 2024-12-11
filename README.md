# Tools required to run the applications.

RabbitMQ is required in local machine to run the application. RabbitMQ can be installed from the installer or can be used from Docker.
Both ways are described below:

****Intall RabbitMQ from Docker:****
+ Install Docker version 4.34.3
+ Install Docker compose
+ Start Docker
+ From command prompt, get into the the directory where the docker-compose.yml file exist. The file is stored in the current repository 'Tools' folder
+ run the command 'docker-compose up -d'

**Intall RabbitMQ from installer:**
- Install Erlang version otp_win64_26.2.5.6
- Install RabbitMQ rabbitmq-server-4.0.4
- Run commad from RabbitMQ command prompt : rabbitmq-plugins enable rabbitmq_management

**After installtion by any of the above way:**
To see the management portal of RabbitMQ
- From browser:  localhost:15672  
- Login with uid:guest  pwd:guest

# About the Application
The application has two parts, called PositionBroadcaster and PositionConsumer.

**PositionBroadcaster :**
Creates position(Lattitude and Longitude. Height is always Zero) inside Netherlands boundary.
The frequency of the position creation is random from 1.01sec to 5sec. 
But, the frequency can be changed from the appsettings.json file.
Another setting in the file called 'DelayInMilliSec' stimulates delayed position publishing. It only affects when the user runs the Broadcaster instance to simulate delayed publishing.
The published messages are logged into the log file.

**PositionConsumer :** 
Receives the positions(Lattitude and Longitude. Height is always Zero) and calculates the average of the positions whose creation time span is within 1 sec.
If any position comes delayed, the average is calculated with the corresponding positions that came earlier. In that case, if the previously calculated aggregate changes then it is notified to the user in the console.

### Improvement Required:
- If any position publish is failed, no retry mechanism has been setup now. So the position is lost.

**Solution:** Retry can be configured. And also Outbox should be created to track and retry the positions again.

- If any aggregate is failed, the position is lost. Not configured to get it again from the event bus. In addition, to that, no way has been implemented to notify the Broadcaster about the status of the consumer processing.

**Solution:** If aggregate fails then maybe store the position locally for later retry or request to dead later. To handle the updates from the Consumer to Broadcaster, Saga can be implemented.





