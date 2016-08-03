open SuaveRestApi.Rest
open SuaveRestApi.Db
open SuaveRestApi.Data
open Suave.Web
open Suave.Successful

[<EntryPoint>]
let main argv = 
    let personWebPart = rest "people" {
        GetAll  = PersonRepository.getPeople
        GetById = PersonRepository.getPersonById
        Create  = PersonRepository.createPerson
        Update  = PersonRepository.updatePerson
        Delete  = PersonRepository.deletePerson
        UpdateById = PersonRepository.updatePersonById
    }

    startWebServer defaultConfig personWebPart 
    0
