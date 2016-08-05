namespace SuaveRestApi.Data

open MongoDB.Bson
open MongoDB.Driver
open MongoDB.FSharp

[<AutoOpen>]
module Data =
    printfn "Connecting to mongodb..."
    let connectionString = "mongodb://localhost"
    let client = new MongoClient(connectionString) :> IMongoClient
    let db = client.GetDatabase "test"
