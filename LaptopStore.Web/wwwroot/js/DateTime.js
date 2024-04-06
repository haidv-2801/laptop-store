function formatDateTime(dateString) {
    // Parse the string date into a Date object
    var date = new Date(dateString);

    // Check if the parsing was successful
    if (isNaN(date.getTime())) {
        return "Invalid Date";
    }

    // Get the components of the date
    var day = date.getDate();
    var month = date.getMonth() + 1; // Month starts from 0, so add 1
    var year = date.getFullYear();
    var hours = date.getHours();
    var minutes = date.getMinutes();
    var seconds = date.getSeconds();
    var ampm = hours >= 12 ? 'PM' : 'AM'; // Determine AM or PM

    // Convert hours from 24-hour format to 12-hour format
    hours = hours % 12;
    hours = hours ? hours : 12; // The hour '0' should be '12' in AM/PM

    // Add leading zeros if necessary
    if (day < 10) {
        day = '0' + day;
    }
    if (month < 10) {
        month = '0' + month;
    }
    if (hours < 10) {
        hours = '0' + hours;
    }
    if (minutes < 10) {
        minutes = '0' + minutes;
    }
    if (seconds < 10) {
        seconds = '0' + seconds;
    }

    // Return the formatted date and time with AM/PM
    return day + '/' + month + '/' + year + ' ' + hours + ':' + minutes + ':' + seconds + ' ' + ampm;
}
