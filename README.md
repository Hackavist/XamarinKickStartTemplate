# Xamarin Kick Starter Template

Get Up and running with your _Xamarin Forms_ project in no time, use this template to get an new xamarin forms project with a MVVM Folder structure, The most used nuget packeges and services with the latest .netstanderd 2.1 and C# 8 with the push of a single button.

## How to use it?

Just Click _Use This Template_ in the Project's Github Repo.

![Xamarin Kick Start Template](https://i.ibb.co/WFczbWN/template.png)

## Why Should You Use it?
The _Xamarin Kick Starter Template_ Cuts the project start overhead by providing you with some of the most commonly used services and nugets packs for every Xamarin Forms Project.

## Basic Features
_This Template is based upon our Own Modified Version Of [Fresh MVVM](https://github.com/rid00z/FreshMvvm), [Xamarin forms](https://github.com/xamarin/Xamarin.Forms) and [Xamarin Essentials](https://github.com/xamarin/Essentials)_.

* MVVM File Structure (with some extra folders for Converters, Custom Controls, etc...).

* ViewModel to ViewModel Navigation

* Automatic wiring of BindingContext

* Automatic wiring of Page events (eg. appearing)

* Basic methods (with values) on PageModel (init, reverseinit)

* Built in IOC Container

* iOS safe area and large titles configrability.

## Additional Features

* Automatic Refresh on Property Change using  [PropertyChanged.Fody](https://github.com/Fody/PropertyChanged).

* Elements State managment using [Xamarin.Forms.StateSquid](https://github.com/sthewissen/Xamarin.Forms.StateSquid).

* User Dialogs and loading Screens using [ACR User Dialogs](https://github.com/aritchie/userdialogs).

* Json Serialization and Deserialization Using [Newtonsoft.Json](https://www.newtonsoft.com/json).

* Media Plugin for Xamarin Using [MediaPlugin](https://github.com/jamesmontemagno/MediaPlugin).

* Weak Event Handeling.

* Async Command Support.

## HomeGrown Features

* In Memory LCS (Longest Common Subsequence) Generic Search.

* SQL Lite Local Database Service.

* Local Notifications Service.

* String formating extensions.

* NullorEmpty checks inside the Objects\*.(for string feilds only).


## More Details on HomeGrown Features

### In Memory LCS (Longest Common Subsequence) Generic Search

All it needs is the collection you what to search in , the name of the property or the expression you are searching for and your search query.

\* All string comparisons are done using the LCS (Longest Common Subsequence) Algorithm more on that [Here](https://www.geeksforgeeks.org/longest-common-subsequence-dp-4/).

``` c#
        /// <summary>
        ///     Returns objects in the collection where its target property is identical or similar to the query string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="targetProperty"></param>
        /// <param name="query"></param>
        /// <returns> Returns IEnumrable of objects in the collection where its target property is identical or similar to the query string</returns>
        public static IEnumerable<T> SearchInCollection<T>(IEnumerable<T> collection, string targetProperty, string query)
```

or 

``` c#
       /// <summary>
        ///     Returns objects in the collection where its target property is identical or similar to the query string
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="targetPropertyFunc"></param>
        /// <param name="query"></param>
        /// <returns>Returns IEnumrable of objects in the collection where its target property is identical or similar to the query string</returns>
        public static IEnumerable<T> SearchInCollection<T>(IEnumerable<T> collection, Func<T, string> targetPropertyFunc,string query)
```

### Local Notification Service

Get local notifications right at ur finger tips with minimal effort using the ``` ILocalNotificationService``` interface.

You can fire it on spot or schedule it for a certain date and time.

``` c#
    public interface ILocalNotificationService
    {
        /// <summary>
        ///     The Executed Behaviour when the app receives a notification
        /// </summary>
        event EventHandler NotificationReceived;

        /// <summary>
        ///     Executes the platform Specific service initiation code
        /// </summary>
        void Initialize();

        /// <summary>
        ///     Show a local notification now
        /// </summary>
        /// <param name="title">Title of the notification</param>
        /// <param name="body">Body or description of the notification</param>
        /// <param name="id">Id of the notification</param>
        void Notify(string title, string body, int id = 0);

        /// <summary>
        ///     Show a local notification at a certain date and time
        /// </summary>
        /// <param name="title">Title of the notification</param>
        /// <param name="body">Body or description of the notification</param>
        /// <param name="notificationDateTime"> The date and time the notification should be fired </param>
        /// <param name="id">Id of the notification</param>
        void Notify(string title, string body, DateTime notificationDateTime, int id = 0);

        /// <summary>
        ///     Cancel a local notification
        /// </summary>
        /// <param name="id">Id of the notification to cancel</param>
        void Cancel(int id);

        /// <summary>
        ///     Channels the parameters to the "NotificationReceived" event when a notification is fired
        /// </summary>
        void ReceiveNotification(string title, string message);
    }
```


### SQL Lite Local Database Service

Get all what you need from a local database just at your finger tips with the ```ILocalDatabaseService``` interface

``` c#
 public interface ILocalDatabaseService
    {
        Task CreateDatabaseTables(List<Type> tables);
        Task CreateDatabaseTables(List<Type> tables,CreateFlags tableCreateFlags);
        Task<List<T>> GetAll<T>() where T : class, new();
        Task<int> Insert(object item);
        Task<int> InsertAll<T>(List<T> items) where T : new();
        Task<int> Update(object item);
        Task<int> UpdateAll<T>(List<T> items) where T : new();
        Task<int> Delete(object obj);
        Task<int> DeleteAll<T>() where T : new();
        Task<int> DropTableAsync<T>() where T : new();
        Task<IList<T>> QueryString<T>(string sql) where T : class, new();
        Task<List<SQLiteConnection.ColumnInfo>> GetTableInfo(string tableName);
        Task<List<T>> GetAll<T>(Expression<Func<T, bool>> query) where T : new();
        Task<T> GetOne<T>(Expression<Func<T, bool>> query) where T : class, new();
        Task InsertOrReplaceAll<T>(List<T> items) where T : new();
        Task<int> InsertOrReplaceOne<T>(object item) where T : new();
        Task DeleteAll<T>(List<T> items) where T : new();
        Task<int> ExecuteAsync(string sqlQuery);
    }
```

## Future Work

* Set MasterDetailsPage Design 

* Async Command Set task Result (to prevent multiple clicks on any giving button)

* On the Fly object validators 

* Basic Converters

* Error Logger

## Issues and Bugs Reporting  

For any issue or bug found please open an _issue_ stating 
1. What was your expected output?

1. What was the actual output?

1. Any supprting screen shots or code snippits 


## Contributing 

Contributing is most welcomed you can contribute by 

1. sending a pull request that includes your changes and suggestions

or 

2. Opening up an issue explaining the suggestion or the feature with an enhancment tag on it

## Special Thanks 

To [Mohamed Ashraf](https://github.com/Ananasa) for his amazing talents in the design





