$(document).ready(function ()
{
    $('select').multiselect(
        {
            includeSelectAllOption: true,
            noneSelectedText: 'Select Something (required)',
            numberDisplayed: 5,
            buttonWidth: '230px'
        });
});

function createReviewersDropdown(articleId)
{
    var container = document.getElementById("reviewersDropDown")
    var selectDiv = document.createElement("select")
    selectDiv.setAttribute("multiple", "multiple")
    selectDiv.setAttribute("id", "selectedReviewers")

    //Get reviwers.
    var reviewers = getReviewers(articleId)   
    appendOptions(selectDiv, reviewers)

    container.appendChild(selectDiv)
}

function appendOptions(selectDiv, reviewers)
{
    var option = null
    for (var item = 0; item < reviewers.length; item++)
    {
        option = document.createElement("option")
        option.innerHTML = reviewers[item]
        selectDiv.appendChild(option)
    }    
}


function getReviewers(articleId)
{
    var reviewers = null
    $.ajax({
        url: '/AssingReviewers/nomeneeEmails',
        data: {
            articleId: articleId,
        },
        type: 'post',
        dataType: 'json',
        async: false,
        success: function (results) {
            reviewers = results;
        }
    });
    return reviewers
}

function saveReviewers(articleId) {
    // Return an array of strings containing the selected options of the given dropdownlist.
    var selectedR = document.getElementById("selectedReviewers")

    var selectedItems = [];
    var len = selectedR.options.length
    var currentOption = null

    for (var item = 0; item < len; ++item) {
        currentOption = selectedR.options[item];
        if (currentOption.selected) {
            selectedItems.push(currentOption.value);
        }
    }
    return selectedItems;
}

function sendToController(articleId)
{
    var reviewersChosen = saveReviewers(articleId)

    if (reviewersChosen.length > 2 && reviewersChosen.length < 6) {
        ajaxReviewers(reviewersChosen, articleId)

        alert("Reviewers succesfully assigned!")
        window.location.href = "/Article/HomePage"
    }
    else
    {
        alert("Reviewers quantity must be between 3 and 5, please select appropiate quantity of reviewers.")
    }

}

function ajaxReviewers(selectedItems, articleId) {
    var reviewers = null
    $.ajax({
        url: '/AssingReviewers/AssingReviewersForm',
        data: {
            checkedEmails: selectedItems,
            articleId: articleId
        },
        type: 'post',
        dataType: 'json',
        async: false,
        success: function (results) {
            reviewers = results;
        }
    });
    return reviewers
}