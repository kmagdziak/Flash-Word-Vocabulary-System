<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
    <style type="text/css">
    .howto
    {
        height: 56px;
    }
        .wordpost
        {
            height: 52px;
        }
        .word_search
        {
            width: 460px;
            height: 63px;
        }
    </style>
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <h2>
        WELCOME TO WORD SEARCH!
    </h2>
    <div class="howto">
        To search for a word, fill in the text box below and press Search.</div>
    <div class="word_db">
        <div style="border-style: solid; border-width: 2px; border-color: #000000; float: left; position: relative; width: 50%; top: 0px; left: 0px; height: 168px; overflow: hidden;" 
            class="word_search">
    Word Search 
                <br />
                <asp:TextBox ID="SearchText" runat="server"></asp:TextBox>
                <asp:Button ID="searchButton" runat="server" Text="Search" 
                onclick="searchButton_Click" /><br />
            <asp:Label ID="WrdSrchExstLabel" runat="server" Text="Word Does Not Exist" 
                ForeColor="Red" Visible="False"></asp:Label>
        </div>
        <div style="border-style: solid; border-width: 2px; border-color: #000000; float: right; position: relative; width: 49%; top: 0px; left: 0px; height: 168px; overflow: visible;" 
            class="word_post">
    <asp:label ID="Spelling" runat="server" AssociatedControlID="SpellingText"> Spelling - </asp:label>
    <asp:TextBox ID="SpellingText" runat="server" TextMode="SingleLine"></asp:TextBox>
            <br />
            <br />
    <asp:label ID="Meaning" runat="server" AssociatedControlID="MeaningText"> Meaning - </asp:label>
    <asp:TextBox ID="MeaningText" runat="server" TextMode="SingleLine"></asp:TextBox>
            <br />
            <br />
    <asp:label ID="Example" runat="server" AssociatedControlID="ExampleText"> Example - </asp:label>
    <asp:TextBox ID="ExampleText" runat="server" TextMode="SingleLine"></asp:TextBox>
            <br />
            <br />
         <asp:Button ID="Save_Word" runat="server" Text="Save Word" 
         InsertCommand="INSERT INTO [Allword] ([Spelling], [Meaning], [Example], [TimeCreated], [TimeUpdated]) VALUES (@Spelling, @Meaning, @Example, @TimeCreated, @TimeUpdated)" 
             onclick="SaveWord_Click" /> 
        </div>
    <br />
    <br />
    <div style="height: 200px; overflow: scroll; border: 2px solid black; position: relative; width: 100%;" 
            class="wordsearchcontent">
         
        <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="False" 
            DataKeyNames="ID" DataSourceID="SqlDataSource1" 
            EmptyDataText="There are no data records to display." AllowSorting="True" >
            <Columns>
                <asp:TemplateField>
                    <EditItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="ID" HeaderText="ID" ReadOnly="True" 
                    SortExpression="ID" />
                <asp:BoundField DataField="Spelling" HeaderText="Spelling" 
                    SortExpression="Spelling" />
                <asp:BoundField DataField="Meaning" HeaderText="Meaning" 
                    SortExpression="Meaning" />
                <asp:BoundField DataField="Example" HeaderText="Example" 
                    SortExpression="Example" />
                <asp:BoundField DataField="TimeCreated" HeaderText="TimeCreated" 
                    SortExpression="TimeCreated" />
                <asp:BoundField DataField="TimeUpdated" HeaderText="TimeUpdated" 
                    SortExpression="TimeUpdated" />
            </Columns>
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:Conn1 %>" 
            DeleteCommand="DELETE FROM [Allword] WHERE [ID] = @ID" 
            InsertCommand="INSERT INTO [Allword] ([Spelling], [Meaning], [Example], [TimeCreated], [TimeUpdated]) VALUES (@Spelling, @Meaning, @Example, @TimeCreated, @TimeUpdated)" 
            ProviderName="<%$ ConnectionStrings:Conn1.ProviderName %>" 
            SelectCommand="SELECT [ID], [Spelling], [Meaning], [Example], [TimeCreated], [TimeUpdated] FROM [Allword]" 
            UpdateCommand="UPDATE [Allword] SET [Spelling] = @Spelling, [Meaning] = @Meaning, [Example] = @Example, [TimeCreated] = @TimeCreated, [TimeUpdated] = @TimeUpdated WHERE [ID] = @ID">
            <DeleteParameters>
                <asp:Parameter Name="ID" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="Spelling" Type="String" />
                <asp:Parameter Name="Meaning" Type="String" />
                <asp:Parameter Name="Example" Type="String" />
                <asp:Parameter Name="TimeCreated" Type="DateTime" />
                <asp:Parameter Name="TimeUpdated" Type="DateTime" />
            </InsertParameters>
            <UpdateParameters>
                <asp:Parameter Name="Spelling" Type="String" />
                <asp:Parameter Name="Meaning" Type="String" />
                <asp:Parameter Name="Example" Type="String" />
                <asp:Parameter Name="TimeCreated" Type="DateTime" />
                <asp:Parameter Name="TimeUpdated" Type="DateTime" />
                <asp:Parameter Name="ID" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>
        <asp:GridView ID="GridView1" runat="server">
        </asp:GridView>
        <br />
     </div>
     </div>
    <asp:Button ID="DeletedSelected" runat="server" onclick="DeleteSelected_Click" 
        Text="Delete Selected" Width="128px" />
    
    <asp:Label ID="Label1" runat="server" Text="Label" Visible="False"></asp:Label>
    <asp:Button ID="ShowNextBtn" runat="server" onclick="ShowNextBtn_Click" 
        Text="Show Next &gt;" />
    <asp:Label ID="Label3" runat="server" Text="Label" Visible="False"></asp:Label>
    <asp:Button ID="ClearSelected" runat="server" onclick="ClearSelected_Click" 
        Text="Clear" Width="116px" />
    <br />      
    </asp:Content>