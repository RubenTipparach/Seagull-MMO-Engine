db.getCollection('Hats').find({})
/* 1 */
{
    "_id" : ObjectId("572ad66a3a493a3168f88f3a"),
    "HatType" : "Fedora"
}

/* 2 */
{
    "_id" : ObjectId("572ad66a3a493a3168f88f3b"),
    "HatType" : "TopHat"
}

/* 3 */
{
    "_id" : ObjectId("572ad66a3a493a3168f88f3c"),
    "HatType" : "Propeller"
}

/* 4 */
{
    "_id" : ObjectId("572ae1c93a493a3168f88f3f"),
    "HatType" : "Fez"



db.getCollection('Messages').find({})

db.getCollection('PlayerInventory').find({})

	/* 1 */
{
    "_id" : ObjectId("572adab53a493a3168f88f3d"),
    "user_id" : ObjectId("572064afb773ea3328a4ffaf"),
    "items" : [ 
        {
            "Hats" : {
                "hatType" : "Fedora"
            },
            "quantity" : 1
        }, 
        {
            "Hats" : {
                "hatType" : "TopHat"
            },
            "quantity" : 1
        }
    ]
}

/* 2 */
{
    "_id" : ObjectId("572adae03a493a3168f88f3e"),
    "user_id" : ObjectId("5721011c815a3c22e511e1a4"),
    "items" : [ 
        {
            "Hats" : {
                "hatType" : "Propeller"
            },
            "quantity" : 2
        }, 
        {
            "Hats" : {
                "hatType" : "Fez"
            },
            "quantity" : 1
        }
    ]
}

db.getCollection('PlayerLocation').find({})
/* 1 */
{
    "_id" : ObjectId("57210061815a3c22e511e1a3"),
    "user_Id" : ObjectId("572064afb773ea3328a4ffaf"),
    "location" : {
        "x" : 245.491973876953,
        "y" : 71.7252578735352,
        "z" : 282.438903808594
    }
}

/* 2 */
{
    "_id" : ObjectId("5721011c815a3c22e511e1a4"),
    "user_id" : ObjectId("5720657cb773ea3328a4ffb0"),
    "location" : {
        "x" : 50,
        "y" : 100,
        "z" : 50
    }
}

db.getCollection('PlayerStats').find({})

/* 1 */
{
    "_id" : ObjectId("572ad4823a493a3168f88f38"),
    "user_Id" : ObjectId("572064afb773ea3328a4ffaf"),
    "experiencePoints" : 0,
    "level" : 1,
    "attributes" : {
        "strength" : 1,
        "agility" : 2,
        "intelligence" : 0
    }
}

/* 2 */
{
    "_id" : ObjectId("572ad5883a493a3168f88f39"),
    "user_Id" : ObjectId("5721011c815a3c22e511e1a4"),
    "experiencePoints" : 0,
    "level" : 1,
    "attributes" : {
        "strength" : 1,
        "agility" : 1,
        "intelligence" : 2
    }
}

db.getCollection('Users').find({})

/* 1 */
{
    "_id" : ObjectId("572064afb773ea3328a4ffaf"),
    "user_id" : 1,
    "name" : "Ruben",
    "age" : 26
}

/* 2 */
{
    "_id" : ObjectId("5720657cb773ea3328a4ffb0"),
    "user_id" : 2,
    "name" : "Anakin",
    "age" : 4
}