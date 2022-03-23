// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
// Could be move to site.js but i will not cuz im really tired :/
$(document).ready(function () {

    // Set today's date when the button click
    $("#today").click(function () {

        document.getElementById("date").value = $('#nowDate').val();
    });

    // Set tomorrow's date when the button click
    $("#tomorrow").click(function () {
        document.getElementById("date").value = $('#tomorrowDate').val();
    });


    // Control "From" change
    var fromSelect = $("#from");
    fromSelect.data("prev", fromSelect.val());

    fromSelect.change(function (data) {
        var jqThis = $(this);

        // Set the first value of "from" before change
        var fromFirst = jqThis.data("prev");

        // Current "to" value
        var to = $('#to :selected').val();

        // Current from
        var currentFrom = $('#from :selected').val()

        if (currentFrom == to) {
            alert("You can't select same location!");
            $('#from').val(fromFirst);
        } else {
            // If selectedfrom is differnt to "to" assing the new previus(prev) value
            jqThis.data("prev", jqThis.val());
        }
    });

    // Control "To" change
    var toSelect = $("#to");
    toSelect.data("prev", toSelect.val());

    toSelect.change(function (data) {
        var jqThis1 = $(this);

        // Set the first value of "to" before change
        var toFirst = jqThis1.data("prev");

        // Current "from" value
        var from = $('#from :selected').val();

        // Current to
        var currentTo = $('#to :selected').val()

        if (currentTo == from) {
            alert("You can't select same location!");
            $('#to').val(toFirst);
        } else {
            // If selectedTo is differnt to "from" assing the new previus(prev) value

            jqThis1.data("prev", jqThis1.val());

        }
    });



    $(".selectData").select2({
        ajax: {
            type: 'GET',
            url: "/Home/GetBusLocations",
            dataType: 'json',
            delay: 250,
            data: function (params) {
                return {
                    key: params.term, // search term
                    page: params.page
                };
            },
            processResults: function (data, params) {
                console.log("liste bu", data);
                // parse the results into the format expected by Select2
                // since we are using custom formatting functions we do not need to
                // alter the remote JSON data, except to indicate that infinite
                // scrolling can be used
                params.page = params.page || 1;

                return {
                    results: data.items,
                    pagination: {
                        more: (params.page * 30) < data.total_count
                    }
                };
            },
            cache: true
        },
        minimumInputLength: 1,
        templateResult: formatRepo,
        templateSelection: formatRepoSelection
    });

    function formatRepo(repo) {
        if (repo.loading) {
            return repo.text;
        }

        // Set the contents of the element with the result data
        var $container = $(
            "<div class='select2-result-repository clearfix'>" +
            "<div class='select2-result-repository__title'></div>" +
            "</div>" +
            "</div>"
        );

        // Set the contents' text
        $container.find(".select2-result-repository__title").text(repo.name);
        return $container;

    }

    function formatRepoSelection(repo) {
        console.log("1", repo);
        return repo.name || repo.text;
    }

    // Swap button click event
    $("#changeLocations").click(function () {

        // Get from and to values
        var fromValue = $('#from :selected').val();
        var toValue = $('#to :selected').val();

        // Clean the list
        $('#from').empty();
        $('#to').empty();

        // Append the "to" and "from" options to their selects
        $('#to').append('<option value="' + fromValue + '" selected>' + document.getElementById('select2-from-container').textContent + '</option>');
        $('#from').append('<option value="' + toValue + '" selected>' + document.getElementById('select2-to-container').textContent + '</option>');

        // Change the previus values of selects
        fromSelect.data("prev", toValue);
        toSelect.data("prev", fromValue);

    });
});