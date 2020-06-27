function initReport() {
    // Obtain dropdown objects.
    var memberRankDropdown = document.getElementById("memberRanks");
    var filtersDropdown = document.getElementById("filters");
    // Obtain selected items in dropdowns.
    var selectedMemberRanks = getSelectedItems(memberRankDropdown);
    var selectedFilters = getSelectedItems(filtersDropdown);
    // Generate report according to selected items.
    generateReport(selectedMemberRanks, selectedFilters);
}

function getSelectedItems(dropdownlist) {
    // Return an array of strings containing the selected options of the given dropdownlist.
    var selectedItems = [];
    var len = dropdownlist.options.length;
    var currentOption = null;

    for (var item = 0; item < len; ++item) {
        currentOption = dropdownlist.options[item];
        if (currentOption.selected) {
            selectedItems.push(currentOption.value);
        }
    }
    return selectedItems;
}

function generateReport(selectedMemberRanks, selectedFilters) {
    var values = null;
    var canvas = null;
    var row = document.getElementById("row");
    row.innerHTML = "";

    // For each filter, get the required data, create a canvas and graphicate it.
    for (var filter = 0; filter < selectedFilters.length; ++filter) {
        values = getFilteredValues(selectedMemberRanks, selectedFilters[filter]);
        canvas = createCanvas();
        drawGraphics(canvas, values);
    }
}

function getFilteredValues(selectedMemberRanks, filter) {
    var filteredValues = null;
    $.ajax({
        url: '/CommunityProgressReport/GetFilteredValues',
        data: {
            selectedMemberRanks: selectedMemberRanks,
            filter: filter,
        },
        type: 'post',
        dataType: 'json',
        async: false,
        success: function (results) {
            filteredValues = results;
        }

    });
    return filteredValues;
}

function createCanvas() {
    // Generate required html elements inside a predefined row, in order to place a canvas. 
    var row = document.getElementById("row");
    var colDiv = document.createElement("div");
    colDiv.setAttribute("class", "col-md-6 py-1");
    var cardDiv = document.createElement("div");
    cardDiv.setAttribute("class", "card");
    var cardBodyDiv = document.createElement("div");
    cardBodyDiv.setAttribute("class", "card-body");
    var canvas = document.createElement("canvas");
    cardBodyDiv.appendChild(canvas);
    cardDiv.appendChild(cardBodyDiv);
    colDiv.appendChild(cardDiv);
    row.appendChild(colDiv);
    return canvas;
}

function drawGraphics(canvas, values) {
    new Chart(canvas, {
        type: 'doughnut',
        data: {
            labels: ["Africa", "Asia", "Europe", "Latin America", "North America"],
            datasets: [
                {
                    label: "Population (millions)",
                    backgroundColor: ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850"],
                    data: values,
                }
            ]
        },
        options: {
            legend: { display: false },
            title: {
                display: true,
                text: 'Predicted world population (millions) in 2050'
            },
            responsive: true,
            maintainAspectRatio: true,
            devicePixelRatio: 2
        }
    });
}

//Functions for user report
function initReportUser() {
    // Obtain dropdown objects.
    var categoriesDropdown = document.getElementById("categories");
    // Obtain selected items in dropdowns.
    var selectedCategories = getSelectedItems(categoriesDropdown);
    generateReportUser(selectedCategories);
}

function generateReportUser(selectedCategories) {
    var values = null;
    var canvas = null;
    var toGraphicate = null;
    var row = document.getElementById("row");
    row.innerHTML = "";
    // For each filter, get the required data, create a canvas and graphicate it.
    for (var categories = 0; categories < selectedCategories.length; ++categories) {
        values = getFilteredValuesUser(selectedCategories[categories]);
        toGraphicate = getValuesAndLabels(values);
        canvas = createCanvas();
        drawColumns(canvas, toGraphicate[0], toGraphicate[1]);
    }
}

function getFilteredValuesUser(category) {
    var categories = null;
    $.ajax({
        url: '/Report/ReportUsers',
        data: {
            category: category,
        },
        type: 'post',
        dataType: 'json',
        async: false,
        success: function (results) {
            categories = results;
        }

    });
    return categories;
}

function getValuesAndLabels(jsonElements) {
    var values = []
    var labels = []
    for (var item = 0; item < jsonElements.length; ++item) {
        values.push(jsonElements[item].y)
        labels.push(jsonElements[item].label)
    }
    return [values, labels]
}

function drawColumns(canvas, values, label) {

    var limit = values.length;
    var dataP = [];
    for (var i = 0; i < limit; i++) {
        dataP.push({ y: values[i], label: label[i] });
    }

    new Chart(canvas, {
        //theme: "light2",
        //animationEnabled: true,
        //title: {
        //    text: "Country Distribution of Community Members"
        //},
        //subtitles: [
        //    { text: "" }
        //],
        //data: [
        //    {
        //        //type: "column", //change type to bar, line, area, pie, etc
        //        //Uncomment below line to add data coming from the controller.
        //        //data: dataP,
        //    }
        //]
        type: 'bar',
        data: {
            labels: label,
            datasets: [
                {
                    label: "Population (millions)",
                    backgroundColor: ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850"],
                    data: values
                }
            ]
        },
    });
}