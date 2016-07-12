/**********************************************************
 * CSCI 473 - Spring 2016
 * 
 * ASSIGNMENT: 4
 * 
 * FILE: Default.aspx.cs
 * 
 * CREATED BY:  Andrew Krutke      - z1756942
 *              Joe LaMantia       - z1741268
 *              Klaudiusz Magdziak - z1673677
 * 
 * DUE DATE: 5/3/16
 * 
 * ABOUT:   Contains the event handlers for the main web
 *          page's body.
 *********************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using wordLib;


public partial class _Default : System.Web.UI.Page
{
    Database db = new Database();   //For interacting with the database
   
    protected void Page_Load(object sender, EventArgs e)
    {

    }    
//DELETE?
/*    //enter a new word on spelling
    protected void spelling_TextChanged(object sender, EventArgs e)
    {

    }
    //enter a new meaning for a word
    protected void meaning_TextChanged(object sender, EventArgs e)
    {

    }
    //enter an example of the word
    protected void example_TextChanged(object sender, EventArgs e)
    {

    } END DELETE*/

    /**************************************************
     * NAME: SaveWord_Click
     * 
     * PARAMETERS: 1. sender: a data object containing
     *                        the source of the event
     *                        to be handled. (?)
     *                        
     *             2. e: an EventArgs object that has
     *                   tools for handling events
     *                   that call this method. (?)
     * 
     * RETURNS: void
     * 
     * FUNCTION: Event handler for the SaveWord button.
     *           On click, this method will save a new
     *           word or update a word if it already 
     *           exists in the database.
     *************************************************/
    protected void SaveWord_Click(object sender, EventArgs e)
    {
        //If the text in the spelling, meaning, and example text boxes are not empty
        if(SpellingText.Text != "" && MeaningText.Text != "" && ExampleText.Text != "")
        {
            //Make a word object with the search result from the database
            Word wordsearch = db.SearchAllWord(SpellingText.Text);

            //If the result was not null
            if (wordsearch != null)
            {
                //Update the table with the new data
                db.UpdateAllWord(SpellingText.Text, MeaningText.Text, ExampleText.Text);
            }
            //Else if the word does not exist
            else
            {
                //Create new word in the database
                db.InsertAllword(SpellingText.Text, MeaningText.Text, ExampleText.Text);
            }

            Response.Redirect(Request.RawUrl);  // Refresh the page
        }
    }

    /************************************************************
     * NAME: DeleteSelected_Click
     * 
     * PARAMETERS: 1. sender: a data object containing
     *                        the source of the event
     *                        to be handled. (?)
     *                        
     *             2. e: an EventArgs object that has
     *                   tools for handling events
     *                   that call this method. (?)
     * 
     * RETURNS: void
     * 
     * FUNCTION: Event handler for the DeleteSelected button.
     *           On click, this method will delete all records
     *           that have the checkboxes checked in the select
     *           records column.
     ***********************************************************/
    protected void DeleteSelected_Click(object sender, EventArgs e)
    {
       
     foreach(GridViewRow row in GridView2.Rows)      //iterate through words checked
        {
            CheckBox cb = (CheckBox)row.FindControl("CheckBox1");       //check if check box is slected
            if (cb != null && cb.Checked)
            {
                string toBeDeleted = row.Cells[2].Text; //get spelling cell contents
                db.DeleteAllWord(toBeDeleted);          //delete word
                Response.Redirect(Request.RawUrl);      //refresh the page
            }
        }
    }

    /************************************************************
     * NAME: searchButton_Click
     * 
     * PARAMETERS: 1. sender: a data object containing
     *                        the source of the event
     *                        to be handled. (?)
     *                        
     *             2. e: an EventArgs object that has
     *                   tools for handling events
     *                   that call this method. (?)
     * 
     * RETURNS: void
     * 
     * FUNCTION: Event handler for the searchButton. On click,
     *           this method will search for a word in the database
     *           and display it in the save word boxes for editing
     *           if it exists. Otherwise if it doesn't exist, then
     *           it will display a null string in the text boxes
     *           and display an error message in a label
     ***********************************************************/
    protected void searchButton_Click(object sender, EventArgs e)
    {
        //create new Word object and search for it in db
        Word newSearch = new Word();

        newSearch = db.SearchAllWord(SearchText.Text);

        //if the Word is found, display it in DetailsView

        if (newSearch != null)
        {
            SpellingText.Text = newSearch.Spelling;
            MeaningText.Text = newSearch.Meaning;
            ExampleText.Text = newSearch.Sample;
            WrdSrchExstLabel.Visible = false;   //set error message to invisible
        }
        else
        {
            SpellingText.Text = "";
            MeaningText.Text = "";
            ExampleText.Text = "";
            WrdSrchExstLabel.Visible = true;    //set error message to visible
        }
    }

    /************************************************************
     * NAME: ShowNextBtn_Click
     * 
     * PARAMETERS: 1. sender: a data object containing
     *                        the source of the event
     *                        to be handled. (?)
     *                        
     *             2. e: an EventArgs object that has
     *                   tools for handling events
     *                   that call this method. (?)
     * 
     * RETURNS: void
     * 
     * FUNCTION: Event handler for the ShowNextBtn_Click. On click,
     *           this method will show the record the user selects
     *           in the GridView2 box.
     *           
     * EXTRA CREDIT: In addition to allownig a user to show a word,
     *               they may now select multiple words and show 
     *               them in a sequence.
     ***********************************************************/
    protected void ShowNextBtn_Click(object sender, EventArgs e)
    {
        List<string[]> showList = new List<string[]>(); //For getting the list of record arrays
        List<string> index = new List<string>();        //For getting the index of the current record
        int showListIndex = 0;                          //For getting the next record

        foreach (GridViewRow row in GridView2.Rows)      //iterate through words
        {
            CheckBox cb = (CheckBox)row.FindControl("CheckBox1");       //check if check box is selected
            if (cb != null && cb.Checked)
            {
                string[] toBeDisplayed = new string[5]; //Make an array for each cell in the record up to example
                for (int i = 0; i <= 4; i++)    //Copy cells into array
                {
                    toBeDisplayed[i] = row.Cells[i].Text;
                }
                index.Add(toBeDisplayed[2]);    //Get the spelling to access its index
                showList.Add(toBeDisplayed);    //Add this array to showList
            }
        }

        //Unless list is empty, begin execution
        if (showList.Count > 0)
        {
            string[] record = new string[5];                        //For obtaining the current record from the list
            showListIndex = index.IndexOf(SpellingText.Text) + 1;   //For getting the index of the next record

            //If the text boxes are NOT null
            if (SpellingText.Text != null )
            {
                //And If the list index is greater than amount of elements in list
                if (showListIndex >= showList.Count)
                {
                    showListIndex = 0;  //Start at beginning of list
                }
            }
            //Else, text boxes are null
            else
            {
                showListIndex = 0;  //Start at beginning of list 
            }
            record = showList[showListIndex];   //Get the record array at the resulting index

            //Display next listing
            SpellingText.Text = record[2];
            MeaningText.Text = record[3];
            ExampleText.Text = record[4];
        }
        //Else, list is empty
        else
        {
            //Display nothing
            SpellingText.Text = "";
            MeaningText.Text = "";
            ExampleText.Text = "";
        }
    }
    /************************************************************
     * NAME: ClearSelected_Click
     * 
     * PARAMETERS: 1. sender: a data object containing
     *                        the source of the event
     *                        to be handled. (?)
     *                        
     *             2. e: an EventArgs object that has
     *                   tools for handling events
     *                   that call this method. (?)
     * 
     * RETURNS: void
     * 
     * FUNCTION: Event handler for the clear button. On click,
     *           sets all checkboxes that are checked in the GridView2
     *           to unchecked.
     ***********************************************************/
    protected void ClearSelected_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in GridView2.Rows)      //iterate through words
        {
            CheckBox cb = (CheckBox)row.FindControl("CheckBox1");       //check if check box is selected
            if (cb != null && cb.Checked)
            {
                cb.Checked = false; //Set checkbox to unchecked
            }
        }
    }
}
