function generateReport()
{
    var memberRankDropdown = document.getElementById("memberRanks");
    var filtersDropdown = document.getElementById("filters");
    var selectedMemberRanks = getSelectedItems(memberRankDropdown);
    var selectedFilters = getSelectedItems(filtersDropdown);
    var values = getValues(selectedMemberRanks, selectedFilters);
}

function getSelectedItems(dropdownlist)
{
    var selectedItems = [];
    var len = dropdownlist.options.length;
    var currentOption = null;

    for (var item = 0;  item < len; ++item)
    {
        currentOption = dropdownlist.options[item];
        if (currentOption.selected)
        {
            selectedItems.push(currentOption.value);
        }
    }
    return selectedItems;
}

function getValues(selectedMemberRanks, selectedFilters)
{
    var valuesList = null;

    $.ajax({
        url: '/CommunityProgressReport/GetValues',
        data:
        {
            selectedMemberRanks: selectedMemberRanks,
            selectedFilters: selectedFilters
        },
        type: 'post',
        dataType: 'json',
        async: false,
        success: function (results) 
        {
            valuesList = results;
        }
    });
    return valuesList;
}

function getGraphic(canvas)
{
    new Chart(canvas, {
        type: 'bar',
        data: {
            labels: ["Africa", "Asia", "Europe", "Latin America", "North America"],
            datasets: [
                {
                    label: "Population (millions)",
                    backgroundColor: ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850"],
                    data: [2478, 5267, 734, 784, 433]
                }
            ]
        },
        options: {
            legend: { display: false },
            title: {
                display: true,
                text: 'Predicted world population (millions) in 2050'
            }
        }
    });
}