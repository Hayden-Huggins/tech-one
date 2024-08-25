export async function getNumericToTextResult(prNumeric)
{
    //could check against a regex statement to validate, throwing error if not valid entry. 
    var jsonPayload = {"NUMBER": prNumeric}
    console.log(jsonPayload)
    var resp = await fetch("http://localhost:3001/", {
      method: "POST", 
      body: JSON.stringify(jsonPayload),
      headers: {
        "Content-Type": "application/json"
      }
    });
    const json = await resp.json()
    return json;
}