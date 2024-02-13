using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

// The API Call Controller
[ApiController]
[Route("german-word-app")]
public class WordsController : ControllerBase {

    // Include the databaseHander to use for retrieving words and word sets
    private readonly DatabaseHandler _databaseHandler;


    public WordsController(DatabaseHandler databaseHandler) {
        _databaseHandler = databaseHandler;
    }

    [HttpPost("get3Words")]
    public IActionResult GetRandomWords(WordRequest request) {
        try {
            // Read database configuration from file
            string configFile = "../database_config.txt";
            _databaseHandler.LoadConfig(configFile);

            // Connect to the database
            _databaseHandler.Connect();

            // Execute query to retrieve three random words of the same word type
            string query = $"SELECT * FROM Words WHERE WordType = '{request.WordType}' ORDER BY RAND() LIMIT 3;";
            var result = _databaseHandler.ExecuteQuery(query);

            // Convert DataTable result to list of words
            List<Word> words = new List<Word>();
            foreach (System.Data.DataRow row in result.Rows) {
                words.Add(new Word {
                    EnglishWord = row["EnglishWord"].ToString(),
                    GermanWord = row["GermanWord"].ToString(),
                    WordType = row["WordType"].ToString()
                });
            }

            _databaseHandler.Disconnect();
            return Ok(words);

        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("getWordSets")]
    public IActionResult GetWordSets() {
        try {
            // Read database configuration from file
            string configFile = "../database_config.txt";
            _databaseHandler.LoadConfig(configFile);

            // Connect to the database
            _databaseHandler.Connect();

            // Execute query to retrieve all word sets
            string query = "SELECT * FROM WordSets;";
            var result = _databaseHandler.ExecuteQuery(query);

            // Convert DataTable result to list of word sets
            List<WordSet> wordSets = new List<WordSet>();
            foreach (System.Data.DataRow row in result.Rows) {
                wordSets.Add(new WordSet {
                    WordSetID = Convert.ToInt32(row["WordSetID"]),
                    WordSetName = row["WordSetName"].ToString()
                });
            }

            _databaseHandler.Disconnect();
            return Ok(wordSets);

        } catch (Exception ex) {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}
