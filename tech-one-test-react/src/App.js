import {useState, useRef, useEffect} from 'react';
import {Row} from 'react-bootstrap';
import { getNumericToTextResult } from './Functions';

function App() {

  const [formInput, setFormInput] = useState({entry: "", result: "", warningMessage: ""});

  var inputRef = useRef(null);

  useEffect(() => {
    //Runs only on the first render
    focusRef(inputRef)
  }, []);

  function handleEntryChange(e)
  {
    var lcFormInput = JSON.parse(JSON.stringify(formInput))
    lcFormInput.entry = e.target.value
    setFormInput(lcFormInput)
  }

  function focusRef(prRef)
  {
    prRef = prRef.current && prRef.current.focus()
  }

  async function validateEntry(prNumeric)
  {
    console.log(prNumeric)
    var lcFormInput = JSON.parse(JSON.stringify(formInput))
    //Throw error if not matching a regex statement, containing numbers and decimals. 
    var regexValidation = /\$?[0-9]+([0-9|,])?(\.[0-9]{1,2})?/

    var validation = regexValidation.test(prNumeric)
    console.log(validation)
    if(validation === false)
    {
      lcFormInput.warningMessage = prNumeric + " is invalid. Please enter a valid entry. Optional $ followed by numbers ending with an optional decimal."
      lcFormInput.warningColour = "red"
      lcFormInput.entry = ""
      focusRef(inputRef)
      setFormInput(lcFormInput) 
      return false
    }
    else
    {
      lcFormInput.warningMessage = "Validation Passed for " + prNumeric
      lcFormInput.warningColour = "green"
      var resp = await getNumericToTextResult(prNumeric)
      console.log(resp)
      lcFormInput.result = resp.Value
    }
    setFormInput(lcFormInput)
  }

  return (
    <div >
      <header>
        <Row><input ref={inputRef} onChange={(e) => handleEntryChange(e)} value={formInput.entry}></input><button className='buttonSubmit' onClick={() => validateEntry(formInput.entry)}>Request</button></Row>
        {formInput.warningMessage !== "" ? <Row><label style={{color: formInput.warningColour}}>{formInput.warningMessage}</label></Row> : ""}
        <Row><label>{"Result: " + formInput.result}</label></Row>
      </header>
    </div>
  );
}

export default App;
