module Testing

let assertEqual expected actual = 
    if expected <> actual then  
        raise (System.Exception("Expected " + expected.ToString() + " but got " + actual.ToString()))