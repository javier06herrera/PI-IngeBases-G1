function searchArticle()
{
    var input = document.getElementById("searchField").value.toLowerCase();
    var table = document.getElementById("userArticles");
    var tableElements = table.getElementsByTagName("tr");
    var currentElement = null; 
    var elementContent = null;

    for(var index = 0; index < tableElements.length; ++index)
    {
        currentElement = tableElements[index].getElementsByTagName("td")[0];
        if(currentElement)
        {
            elementContent = (currentElement.textContent || currentElement.innerText).toLowerCase();
            if(elementContent.indexOf(input) > -1)
            {
                tableElements[index].style.display = "";
            }
            else
            {
                tableElements[index].style.display = "none";
            }
        }       
    }
}