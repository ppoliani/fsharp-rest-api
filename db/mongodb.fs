namespace SuaveRestApi.Data

open MongoDB.Bson
open MongoDB.Driver
open MongoDB.FSharp

[<AutoOpen>]
module Data =
    printfn "Connecting to mongodb..."
    Serializers.Register()

    let connectionString = "mongodb://localhost"
    let client = new MongoClient(connectionString)
    let db = client.GetDatabase("test")
