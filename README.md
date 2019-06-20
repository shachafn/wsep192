# AviExpress
An E-Commerce website built using .Net Core and MongoDB.

## How To Run
How to Run Mongo:
  Windows:
  1. [Download MongoDB Server](https://www.mongodb.com/download-center/community)
  2. [Add MongoDB to the system's path](https://dangphongvanthanh.wordpress.com/2017/06/12/add-mongos-bin-folder-to-the-path-environment-variable/) 
  3. Create the path C:\MongoDB\Replica1.
  4. Open the Command Line as an Administrator and run: “mongod --replSet rs1 --dbpath C:\MongoDB\Replica1 --port 37017”
  5. Open the Command Line as an Administrator and run: “mongo --port 37017”
  6. In the Command Line from step 5 run :” config = { _id: "rs1", members: [ {_id: 0, host: "localhost:37017"} ] }”
  7. In the Command Line from step 5 run :”rs.initiate(config)”
