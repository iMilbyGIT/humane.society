INSERT INTO dbo.Categories VALUES(1,'Dog');
INSERT INTO dbo.Categories VALUES(2,'Cat');
INSERT INTO dbo.Categories VALUES(3,'Reptile');
INSERT INTO dbo.Categories VALUES(4,'Mythology');
INSERT INTO dbo.Categories VALUES(5,'Insect');

INSERT INTO dbo.DietPlans VALUES(01,'Carnivore','Meat',3);
INSERT INTO dbo.DietPlans VALUES(02,'Herbivore','Plant',4);
INSERT INTO dbo.DietPlans VALUES(03,'Omnivore','Meat/Plant',2);
INSERT INTO dbo.DietPlans VALUES(04,'Mystic','TBD',5);
INSERT INTO dbo.DietPlans VALUES(05,'Microbe','Plankton',1);

INSERT INTO dbo.Animals VALUES(001,'Lucky The Lab',80,5,'CALM',1,1,'Male','Needs Home',1,01, enterEMPid);
INSERT INTO dbo.Animals VALUES(002,'Kat the Cat',18,12,'MAJESTIC',0,1,'Female','Needs Home',2,01, enterEMPid);
INSERT INTO dbo.Animals VALUES(003,'Igg The Iguana',4,2,'ANXIOUS',1,0,'Male','Needs Home',3,01, enterEMPid);
INSERT INTO dbo.Animals VALUES(004,'Sabah The Sphynx',200000,5000,'LOGICAL',1,1,'Non-Binary','Adopted',4,04, enterEMPid);
INSERT INTO dbo.Animals VALUES(005,'Camie The Chimera',1000,400,'HOSTILE',0,0,'Female','Needs Home',4,04, enterEMPid);

INSERT INTO dbo.Rooms VALUES(1,11, 001);
INSERT INTO dbo.Rooms VALUES(2,21, 002);
INSERT INTO dbo.Rooms VALUES(3,31, 003);
INSERT INTO dbo.Rooms VALUES(4,41, 004);
INSERT INTO dbo.Rooms VALUES(5,51, 005);
INSERT INTO dbo.Rooms VALUES(6,61, 005);
INSERT INTO dbo.Rooms VALUES(7,71, 004);
INSERT INTO dbo.Rooms VALUES(8,81, 003);
INSERT INTO dbo.Rooms VALUES(9,91, 002);
INSERT INTO dbo.Rooms VALUES(10,101, 001);

INSERT INTO dbo.Employees VALUES(987,'Dog The','Bounty Hunter','DBHbark','P@ssword',987987,'dbh@notactuallyhunting.com');
INSERT INTO dbo.Employees VALUES(654,'Mark','Wahlberg','Mark-e-Mark','G00dV!bes',071991,'imfrombostoncanyoutell@stfu.com');
INSERT INTO dbo.Employees VALUES(321,'Dog The','Bounty Hunter','DBHbark','P@ssword',987987,'dbh@notactuallyhunting.com');
INSERT INTO dbo.Employees VALUES(753,'Dog The','Bounty Hunter','DBHbark','P@ssword',987987,'dbh@notactuallyhunting.com');
INSERT INTO dbo.Employees VALUES(159,'Donald','Trump','DBHbark','P@ssword',987987,'dbh@notactuallyhunting.com');

INSERT INTO dbo.Clients VALUES('Chimera');
INSERT INTO dbo.Clients VALUES('Chimera');
INSERT INTO dbo.Clients VALUES('Chimera');
INSERT INTO dbo.Clients VALUES('Chimera');
INSERT INTO dbo.Clients VALUES('Chimera');
