# C_Sharp_Sql_Manager

## This is a library simplyfing the use of databases in c# applications.

## Documentation:
### Create new sqlManager:
```csharp
using SqlLibrary;
SqlManager sqlManager = new SqlManager();
```


### Databases:

#### Create a new Database:
```csharp
sqlManager.CreateDatabase(Database_name, Connection_string, Datbase_folder);
//Database_name is the name of the database that is going to be created

//Connection_string is the connection string that will be used for the creation of the database.
//Leave empty for the default:
//Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=true;

//Datbase_folder is the folder that the .mdf file will be created in.
//Leave empty for the default:
//AppDomain.CurrentDomain.BaseDirectory

//Note: the files created will be the Database_name with extension.
//Example: if the Database_name is "Database" the files created will be named:
// Database_data.mdf
// Database_logs.ldf
```

#### Delete Database:
```csharp
sqlManager.DeleteDatabase(Database_name, Connection_string);
//Database_name is the name of the database that is going to be deleted

//Connection_string is the connection string that will be used for the deleting of the database.
//Leave empty for the default:
//Data Source=(LocalDB)\MSSQLLocalDB;Integrated Security=true;
```


### Tables:

#### Create Table:
```csharp
sqlManager.CreateTable(Table_name, List_of_Coloumns, Connection_string);
//Table_name is the name of the table that is going to be created

//List of coloumns is just a list of strings containing the neccessary information for the coloumn. Example:
List<string> List_of_Coloumns = new List<string>();
List_of_Coloumns.Add("[index][int] NOT NULL PRIMARY KEY");
List_of_Coloumns.Add("[Name] [varchar](100) NULL");

//Connection_string is the connection string that is going to be used for the creation of the table
```

#### Delete Table
```csharp
sqlManager.DeleteTable(Table_name, Connection_string);
//Table_name is the name of the table that is going to be deleted

//Connection_string is the connection string that is going to be used for the deleting of the table
```


### Insert, Update, Delete:

#### Insert:
```csharp
sqlManager.Insert(ColoumnName_ValueToInsert, Table_name, Connection_string);
//ColoumnName_ValueToInsert is a list containing both the coloumn that the data is going to be inserted into, as well as the data that is going to be inserted into that coloumn.

//Table_name is the name of the table that the data is going to be inserted into.

//Connection_string is the connection string that is going to be used for the insertion.

//Example: If we wanted to insert the values 1 & "Alan" into the table we created before, we would do it like this:
List<Tuple<dynamic, dynamic>> ColoumnName_ValueToInsert = new List<Tuple<dynamic, dynamic>>();
ColoumnName_ValueToInsert.Add(new Tuple<dynamic, dynamic>("index", 1));
ColoumnName_ValueToInsert.Add(new Tuple<dynamic, dynamic>("Name", "Alan"));
sqlManager.Insert(ColoumnName_ValueToInsert, Table_name, Connection_string);
```

#### Update:
```csharp
sqlManager.Update(ColoumnName_ValueToInsert, ColoumnName_ValueToMatch, Table_name, Connection_string);
//ColoumnName_ValueToInsert is a list containing both the coloumn where the data is going to be updated, as well as the data that is going to be updated in that coloumn.

//ColoumnName_ValueToMatch is a list containing both the coloumn where we want to find our match, as well as the data that we want to match in that coloumn.

//Table_name is the name of the table where the data is going to be updated.

//Connection_string is the connection string that is going to be used for the updating.

//Example: If we wanted to update the values 4 & "Elom" to 4 & "Elon" in the table we created before, we would do it like this:
List<Tuple<dynamic, dynamic>> ColoumnName_ValueToInsert = new List<Tuple<dynamic, dynamic>>();
List<Tuple<dynamic, dynamic>> ColoumnName_ValueToMatch = new List<Tuple<dynamic, dynamic>>();
ColoumnName_ValueToInsert.Add(new Tuple<dynamic, dynamic>("Name", "Elon"));
ColoumnName_ValueToMatch.Add(new Tuple<dynamic, dynamic>("index", 4));
sqlManager.Update(ColoumnName_ValueToInsert, ColoumnName_ValueToMatch, Table_name, Connection_string);
```

#### Delete:
```csharp
sqlManager.Delete(ColoumnName_ValueToMatch, Table_name, Connection_string);
//ColoumnName_ValueToMatch is a list containing both the coloumn that we want to find our match in as well as the data that we want to match.

//Table_name is the name of the table in which the data will be deleted in

//Connection_string is the connection string that is going to be used to delete the data

//Example: If we wanted to delete the row containing the data 6 & "Albert" in the table we created before, we would do it like this:
List<Tuple<dynamic, dynamic>> ColoumnName_ValueToMatch = new List<Tuple<dynamic, dynamic>>();
ColoumnName_ValueToMatch.Add(new Tuple<dynamic, dynamic>("index", 6));
ColoumnName_ValueToMatch.Add(new Tuple<dynamic, dynamic>("Name", "Albert"));
sqlManager.Delete(ColoumnName_ValueToMatch, Table_name, Connection_string);
```


### Backups:

#### Create Backup:
```csharp
sqlManager.Backup(Connection_string);
//Connection_string is the connection string that will be used for the backup

//Note: this will automaticly open a save file dialog and backup your entire database to a .bak file
```

#### Restore Backup:
```csharp
sqlManager.Restore(Connection_string);
//Connection_string is the connection string that will be used for the restoration

//Note: this will automaticly open an open file dialog and restore the database selected by the user to the database specified in the connection string
```


### Special Features:

#### BindGirdView()
##### This returns a data table wich you can bind to your gridview, in simple terms, you can use this to display a table in a gridview.
```csharp
dataGridView.DataSource = sqlManager.BindGridView(Table_name, Connection_string);
//dataGridView is a dataGridView which you have already created

//Table_name is the name of the table that is going to be displayed in the dataGridView

//Connection_string is the connection string that is going to be used to bind the dataGridView
```


### Execute a Command:
```csharp
sqlManager.ExecuteSqlCommand(Sql_Command, Connection_string);
//Sql_Command is a string containing the command that you want to execute

//Connection_string is the connection string that is going to be used to execute the command
```
