import requests
import json

request = requests.get("https://swapi.dev/api/people/")
print(request.text)

# Check if correct status code
assert request.status_code == 200, "Wrong request status code returned"

# Check if correct data structure returned
try:
    response = json.loads(request.text)
except json.JSONDecodeError as a:
    assert False, "Wrong data format returned"

# Check if response main fields are in
for i in ["count", "next", "previous", "results"]:
    assert i in response, "Key is not in a response"
    if i == 'results':
        for human in response['results']:
            assert (key in
                    ["name", "birth_year", "eye_color", "gender", "hair_color", "height", "mass",
                     "skin_color", "homeworld", "films", "species", "starships", "vehicles", "url",
                     "created", "edited"] for key in human.keys())
            for a in human.values():
                print(a)
            assert (type(value_type) == str for value_type in human.values())
            try:
                int(human["height"])
                int(human["mass"])
            except ValueError:
                assert False, "Wrong value type returned"
            for url in human["films"]:
                try:
                    film = requests.get(url)
                    assert film.status_code == 200, "Broken film url"
                except ValueError:
                    print()



