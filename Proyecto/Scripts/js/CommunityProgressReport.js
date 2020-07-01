function initReport()
{
	// Obtain dropdown objects.
	//var memberRankDropdown = document.getElementById("memberRanks");
	var filtersDropdown = document.getElementById("filters");
	// Obtain selected items in dropdowns.
	//var selectedMemberRanks = getSelectedItems(memberRankDropdown);
	var selectedFilters = getSelectedItems(filtersDropdown);
	// Generate report according to selected items.
	//generateReport(selectedMemberRanks, selectedFilters);
	generateReport(selectedFilters);
}

function getSelectedItems(dropdownlist)
{
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

//function generateReport(selectedMemberRanks, selectedFilters)
//{
//    var values = null;
//    var canvas = null;
//    var row = document.getElementById("row");
//    row.innerHTML = "";

//    // For each filter, get the required data, create a canvas and graphicate it.
//    for (var filter = 0; filter < selectedFilters.length; ++filter) {
//        values = getFilteredValues(selectedMemberRanks, selectedFilters[filter]);
//        canvas = createCanvas();
//        drawGraphics(canvas, values);
//    }
//}

function generateReport(selectedFilters) {
	var values = null;
	var canvas = null;
	var row = document.getElementById("row");
	row.innerHTML = "";
	var toGraphicate = null;
	// For each filter, get the required data, create a canvas and graphicate it.
	for (var filter = 0; filter < selectedFilters.length; ++filter) {
		if (selectedFilters[filter] == "Number of articles peer category and topic" || selectedFilters[filter] == "Access Count peer category and topic") {
			values = getFilteredTable(selectedFilters[filter]);
			canvas = createCanvas();
			toGraphicate = extractDataSet(values);
            drawStackedColumns(canvas, toGraphicate[0], toGraphicate[1], toGraphicate[2], selectedFilters[filter]);
		}
		else {
			values = getFilteredValues(selectedFilters[filter]);
			canvas = createCanvas();
			toGraphicate = getValuesAndLabels(values);
            drawColumns(canvas, toGraphicate[0], toGraphicate[1], selectedFilters[filter]);
		}
		//drawGraphics(canvas, values);        
	}
}

function extractDataSet(values) {
 //   var resultTopic = [];
 //   var resultCategory = [];
 //   labelTopic = [];
 //   labelCategory = [];
 //   members = [];
 //   var hashtag = false;
	//for (var columns = 1; columns < values[0].length; ++columns) {
 //       if (values[1][columns] == "#") {
 //           hashtag = true;
 //       }

 //       else if (!hashtag) {
 //           group = [];
 //           for (var rows = 1; rows < values.length; ++rows) {
 //               group.push(parseInt(values[rows][columns]))

 //           }
 //           labelTopic.push(values[0][columns])
 //           resultTopic.push(group)
 //       }

 //       else if (hashtag) {
 //           group = [];
 //           for (var rows = 1; rows < values.length; ++rows) {
 //               group.push(parseInt(values[rows][columns]))

 //           }
 //           labelCategory.push(values[0][columns])
 //           resultCategory.push(group)
 //       }
    var result = [];
    var label = [];
    var members = [];
    var hashtag = false;
    for (var columns = 1; columns < values[0].length; ++columns) {
        if (values[1][columns] == "#") {
            hashtag = true;
        }

        else if (!hashtag) {
            group = [];
            for (var rows = 1; rows < values.length; ++rows) {
                group.push(parseInt(values[rows][columns]))

            }
            label.push(values[0][columns]);

            for (var rows = 1; rows < values.length; ++rows) {
                group.push(0);
            }
            result.push(group);
        }

        else if (hashtag) {
            group = [];
            for (var rows = 1; rows < values.length; ++rows) {
                group.push(0);
            }
            for (var rows = 1; rows < values.length; ++rows) {
                group.push(parseInt(values[rows][columns]));

            }
            label.push(values[0][columns]);
            result.push(group);
        }

	}
	for (var rows = 1; rows < values.length; ++rows) {
		members.push(values[rows][0] + "Topics");
    }
    for (var rows = 1; rows < values.length; ++rows) {
        members.push(values[rows][0] + "Category");
    }
    return [result, label, members];
}


//Glori
//function getFilteredValues(selectedMemberRanks, filter)
//{
//    var filteredValues = null;
//    $.ajax({
//        url: '/CommunityProgressReport/GetFilteredValues',
//        data: {
//            selectedMemberRanks: selectedMemberRanks,
//            filter: filter,
//        },
//        type: 'post',
//        dataType: 'json',
//        async: false,
//        success: function (results) {
//            filteredValues = results;
//        }

//    });
//    return filteredValues;
//}

//Nueva sin miembros
function getFilteredValues(filter) {
	var filteredValues = null;
	$.ajax({
		url: '/CommunityProgressReport/GetFilteredValues',
		data: {
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

function getFilteredTable(filter) {
	var filteredValues = null;
	$.ajax({
		url: '/CommunityProgressReport/GetFilteredTable',
		data: {
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

function createCanvas()
{
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

function drawGraphics(canvas, values)
{
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
	if (selectedCategories.length > 0) {
		generateReportUser(selectedCategories);
	}
	else {
		alert("Please select an option!");
	}
}

function generateReportUser(selectedCategories) {
	var values = null;
	var canvas = null;
	var title = null
	var toGraphicate = null;
	var row = document.getElementById("row");
	row.innerHTML = "";
	// For each filter, get the required data, create a canvas and graphicate it.
	for (var categories = 0; categories < selectedCategories.length; ++categories) {
		values = getFilteredValuesUser(selectedCategories[categories]);
		title = "Distribution of " + selectedCategories[categories].toLowerCase() + " for community members";
		toGraphicate = getValuesAndLabels(values);
		canvas = createCanvas();
		drawColumns(canvas, toGraphicate[0], toGraphicate[1], title);
	}
}

function getFilteredValuesUser(category) {
	var categories = null;
	var jsonString = null;
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

function drawColumns(canvas, values, label, graphicTitle) {

	var limit = values.length;
	var dataP = [];
	for (var i = 0; i < limit; i++) {
		dataP.push({ y: values[i], label: label[i] });
	}

	new Chart(canvas, {
		type: 'bar',
		data: {

            labels: label,            
			datasets: [
                {					
					backgroundColor: ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850"],
                    data: values,                    
				}
			]
		},
		options: {
			scales: {
				yAxes: [{
					ticks: {
						beginAtZero: true
					}
				}]
            },
            title: {
                display: true,
                text: graphicTitle
            },
            legend: {
                display: false
            },
            tooltips: {
                callbacks: {
                    label: function (tooltipItem) {
                        return tooltipItem.yLabel;
                    }
                }
            }
		},
	});
}

function drawStackedColumns(canvas, result, label, members, graphicTitle)
{
    var limit = result.length;
    var dataTopic = [];

    //var dynamicColors = function () {
    //    var r = Math.floor(Math.random() * 255);
    //    var g = Math.floor(Math.random() * 255);
    //    var b = Math.floor(Math.random() * 255);
    //    return "rgb(" + r + "," + g + "," + b + ")";
    //};
    const colors = randomColor({ count: limit });

	for (var i = 0; i < limit; i++) {
        //dataP.push({ type: "stackedcolumn", name: label[i], showInLegend : "true", datapoints: values[i]  });
        //dataTopic.push({ label: label[i], data: result[i], backgroundColor: dynamicColors() });
        dataTopic.push({ label: label[i], data: result[i], backgroundColor: colors[i] });
    }

    var chart = new Chart(canvas, {
        type: 'bar',
        data: {
            labels: members,
            
            datasets: dataTopic,
                //[
            //    {
            //        label: 'Low',
            //        data: [67.8, 12.0],
            //        backgroundColor: '#D6E9C6' // green
            //    },
            //    {
            //        label: 'Moderate',
            //        data: [20.7, 7.8],
            //        backgroundColor: '#FAEBCC' // yellow
            //    },
            //    {
            //        label: 'High',
            //        data: [11.4, 1.2],
            //        backgroundColor: '#EBCCD1' // red
            //    }
            //],
        },
        options: {
            scales: {
                xAxes: [{
                    stacked: true
                }],
                yAxes: [{
                    stacked: true
                }]
            },
            title: {
                display: true,
                text: graphicTitle
            },
        }
    });
    chart.data.labels.push

}