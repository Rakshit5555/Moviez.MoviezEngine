# Moviez.MoviezEngine
**Endpoint Info:**
https://user-images.githubusercontent.com/51914073/188324924-6863e13d-01d8-404e-8a7e-8fe349f02a10.png

Sample Responses:
1. GET: api/actorslist (For dropdowns selections)
                    [
                      {
                        "actorId": 1,
                        "actorName": "Daniel Craig"
                      },
                      {
                        "actorId": 2,
                        "actorName": "Emma Watson"
                      },
                      {
                        "actorId": 3,
                        "actorName": "Jackie Chan"
                      },
                      {
                        "actorId": 4,
                        "actorName": "Tom Cruise"
                      },
                      {
                        "actorId": 7,
                        "actorName": "Selena Gomez"
                      }
                    ]
     
2. GET: api/movies
                    [
                      {
                        "movieName": "The Spectre",
                        "dateOfRelease": "2015-11-20T00:00:00+05:30",
                        "producerName": "p_name",
                        "posterLink": "link.com",
                        "plot": "sample_plot",
                        "actors": [
                          {
                            "actorName": "Daniel Craig"
                          },
                          {
                            "actorName": "Emma Watson"
                          },
                          {
                            "actorName": "Jackie Chan"
                          }
                        ]
                      },
                      {
                        "movieName": "The Twist",
                        "dateOfRelease": "2015-11-22T00:00:00+05:30",
                        "producerName": "Carrie",
                        "posterLink": "3link.com",
                        "plot": "sample plot for Twist",
                        "actors": [
                          {
                            "actorName": "Daniel Craig"
                          },
                          {
                            "actorName": "Emma Watson"
                          },
                          {
                            "actorName": "Selena Gomez"
                          }
                        ]
                      }
                    ]
              
3. GET: api/producerslist
                     [
                      {
                        "producerId": 2,
                        "producerName": "Jim"
                      },
                      {
                        "producerId": 3,
                        "producerName": "Carrie"
                      }
                    ]
4. PUT: api/movies
                    {
                        "status": true,
                        "message": "Movie Details Updated Successfully"
                    }
5. POST: api/movies
                    {
                        "status": true,
                        "message": "Movie Details Added Successfully"
                    }
