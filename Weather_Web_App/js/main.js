//wrappig JavaScript code in a DOMContentLoaded to ensure the HTML document is fully loaded
//before executing it to prevent any null reference error
document.addEventListener('DOMContentLoaded', function () {
    //api url
    const apiURL = "https://localhost:44314/api/WeatherAPI";
    //function to fetch and display weather data
     async function getWeatherData()
    {
        try
        {
            const response = await fetch(apiURL, {
                method: 'GET',
                headers: {'Content-Type':'application/json'},
                mode: 'cors'
            });
            const data = await response.json();
            console.log(data);
            displayWeatherData(data);
        }
        catch (error)
        {
            console.error("Error fetching weather data.", error);
        }
    }
    //function to display weather data in table
    function displayWeatherData(data)
    {
        const weatherDataElement = document.getElementById("weatherData");
        //clear previous data
        weatherDataElement.innerHTML="";
        //create table rows for each element
        data.forEach(weather => {
            const row = weatherDataElement.insertRow();
            //using event delegation for the delete and update buttons to ensure that the functions are defined
            row.innerHTML = 
                `<td>${weather.date}</td>
                <td>${weather.country}</td>
                <td>${weather.city}</td>
                <td>${weather.condition}</td>
                <td>${weather.temperature}</td>
                <td>
                    <button data-weather-id="${weather.weatherID}" class="edit-btn">Edit</button>
                    <button data-weather-id="${weather.weatherID}" class="delete-btn">Delete</button>
                </td>`;
        });
    }
    //function to sort by date and time
    function sortByDateTime(order) 
    {
        const weatherDataElement = document.getElementById("weatherData");
        const rows = Array.from(weatherDataElement.rows);

        rows.sort((a, b) => 
        {
            const dateA = new Date(a.cells[0].textContent);
            const dateB = new Date(b.cells[0].textContent);

            if (order === 'asc') 
            {
                return dateA - dateB;
            } 
            else 
            {
                return dateB - dateA;
            }
        });

        // Clear and re-append rows based on the sorted order
        weatherDataElement.innerHTML = "";
        rows.forEach(row => weatherDataElement.appendChild(row));
    }

    //function to delete weather data
    async function deleteWeather(id)
    {
        try
        {
            const response = await fetch(`${apiURL}/${id}`,
            {
                method: "DELETE",
                headers: {'Content-Type':'application/json'},
                mode: 'cors'
            });
            //check if the request was successful
            if (response.status === 204) 
            {
                //refresh weather data on the page after deleting
                getWeatherData();
            } 
            else
            {
                console.error(`Error deleting weather entry with ID ${id}.`);
            }
        } 
        catch (error) 
        {
        console.error("Error deleting weather entry.", error);
        }
    }
    //function to edit weather
    async function editWeather(id)
    {
        //fetch the weather data by id
        const weatherToEdit = await fetch(`${apiURL}/${id}`).then(response => response.json());
        //create a form dynamically
        const editForm = document.createElement("form");
        editForm.id = "editWeatherForm";
        //array of labels and input elements to add to form
        const formElements = [
            {label: "Date and Time:", type: "datetime-local", id: "editDate", value:weatherToEdit.date, required:true},
            {label: "Country:", type:"text", id:"editCountry", value: weatherToEdit.country, required: true},
            {label: "City:", type:"text", id:"editCity", value:weatherToEdit.city, required: true},
            {label: "Weather Conditions:", type: "text", id: "editCondition", value: weatherToEdit.condition, required: true},
            {label: "Temperature:", type:"number", id:"editTemperature", value: weatherToEdit.temperature, required: true}
        ];
        //dynamically creating elements for each label and input from array
        formElements.forEach(element => {
            const label = document.createElement("label");
            label.textContent = element.label;
            const input = document.createElement("input");
            input.type = element.type;
            input.id = element.id;
            input.value = element.value;
            input.required = element.required;
            editForm.appendChild(label);
            editForm.appendChild(input);
        });
        //creating a button to submit changes
        const updateButton = document.createElement("button");
        updateButton.textContent= "Update Weather";
        updateButton.onclick = async () =>
        {
            //storing updated data
            const updatedData =
            {
                weatherID: id,
                date: document.getElementById("editDate").value,
                country: document.getElementById("editCountry").value,
                city: document.getElementById("editCity").value,
                condition: document.getElementById("editCondition").value,
                temperature: document.getElementById("editTemperature").value
            };
            //updating weather data
            try
            {
                const response = await fetch(`${apiURL}/${id}`, {
                    method: "PUT",
                    headers: {"Content-Type":"application/json",},
                    body: JSON.stringify(updatedData),
                });
                //refresh weather data after updating
                getWeatherData();
                //remove the form when not needed
                editForm.remove();
            }
            catch
            {
                console.error("Error updating weather entry with ID ${id}.");
            }
        };
        editForm.appendChild(updateButton);
        document.body.appendChild(editForm);
    }
    //function to add weather
    async function addWeatherData(event)
    {
        event.preventDefault();
        const date = document.getElementById("datetime").value;
        const country = document.getElementById("country").value;
        const city = document.getElementById("city").value;
        const condition = document.getElementById("conditions").value;
        const temperature = document.getElementById("temperature").value;
        try
        {
            const response = await fetch(apiURL,
                {
                    method: "POST",
                    headers:{"Content-Type":"application/json"},
                    mode: 'cors',
                    body: JSON.stringify({
                        date: date,
                        country: country,
                        city: city,
                        condition: condition,
                        temperature: temperature
                    }),
                });
                //Refresh weather data after adding
                getWeatherData();
        }
        catch
        {
            console.error("Error adding weather data.");
        }
    }
    //display weather on page
    getWeatherData();
    //event listener for form submission
    document.getElementById("addWeatherForm").addEventListener("submit", addWeatherData);
    //event listener for delete and update buttons clicks using event delegation
    document.addEventListener('click', function(event)
    {
        const weatherID = event.target.getAttribute('data-weather-id');
        if (event.target.classList.contains('delete-btn')) 
        {
            deleteWeather(weatherID);
        } 
        else if (event.target.classList.contains('edit-btn')) 
        {
            editWeather(weatherID);
        }
    });
    //event listener for sorting buttons
    document.getElementById("sortAscBtn").addEventListener("click", function () 
    {
        sortByDateTime('asc');
    });

    document.getElementById("sortDescBtn").addEventListener("click", function () 
    {
        sortByDateTime('desc');
    });
});
