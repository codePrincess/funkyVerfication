#r "Newtonsoft.Json"

using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

public static TraceWriter logger = null;

public static async Task<object> Run(HttpRequestMessage req, TraceWriter log)
{    
    string myContent = req.Content.ReadAsStringAsync().Result;

    logger = log;

    if (myContent == String.Empty) {
        log.Info("no image found");
        return req.CreateResponse(HttpStatusCode.BadRequest, new {
            error = "Image might be empty"
        });
    }

    string apiKey = "4d761377672d4b3ca55fe17274c41f87";
    string csOCRUrl = "https://westus.api.cognitive.microsoft.com/vision/v1.0/ocr";

    HttpClient client = new HttpClient();
    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/octet-stream"));

    HttpContent content = req.Content;
    content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/octet-stream");

    HttpResponseMessage result = await client.PostAsync(csOCRUrl, content);
    
    var csData = result.Content.ReadAsStringAsync().Result;

    if (csData != null) {
        var idtype = "Unknown";

        Dictionary<string, object> values = JsonConvert.DeserializeObject<Dictionary<string, object>>(csData);
        
        foreach(string key in values.Keys) {
            dynamic value = values[key];

            if (key == "regions") {
                idtype = ValidateIdType(value, log);
            }
        }

        return req.CreateResponse(HttpStatusCode.OK, new {
            TypeOfId = idtype,
            code = HttpStatusCode.OK
        });
    }
    else {
        return req.CreateResponse(HttpStatusCode.OK, new {
            TypeOfId = "Unknown",
            code = HttpStatusCode.UnsupportedMediaType
        });
    }
    
}

public static string ValidateIdType (dynamic regionData, TraceWriter log) {

    int [] persoBirthdate = {472,297,195,30};
    int [] passBirthdate = {506,191,120,14};

    foreach (dynamic box in regionData) {
        var lines = box["lines"];

        foreach (dynamic line in lines) {
            var words = line["words"];

            foreach (dynamic word in words) {

                logger.Info("word --> " + word.ToString());

                string wordbox = word["boundingBox"];
                string[] rectValues = wordbox.Split(',');

                var intRectValues = Array.ConvertAll(rectValues, item => Int32.Parse(item));

                bool persoSuccess = matchBoundingBoxes(intRectValues, persoBirthdate, log);
                bool passSuccess = matchBoundingBoxes(intRectValues, passBirthdate, log);

                if (persoSuccess == true) {
                    return "Perso";
                } else if (passSuccess == true) {
                    return "Passport";
                }
            }
        }
    }

    return "Unknown";
}

private static bool matchBoundingBoxes (int[] rect, int[] model, TraceWriter log) {
    var deviation = 10;
    bool success = false;

    for (int i = 0; i < 4; i++) {
        if (Math.Abs(rect[i] - model[i]) < deviation) {
            success = true;
        } else {
            success = false;
            break;
        }
    }

    log.Info("comparing: rect="+(string.Join(" ", rect))+" -- model = "+(string.Join(" ", model))+" -- with success: "+success);
    return success;
}
