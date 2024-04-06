function formatNumber(number) {
    // Convert the number to a string
    var strNumber = number.toString();

    // Split the number into integer and fractional parts (if any)
    var parts = strNumber.split('.');

    // Format the integer part with commas for thousands separator
    parts[0] = parts[0].replace(/\B(?=(\d{3})+(?!\d))/g, ",");

    // Join the integer and fractional parts with a dot
    return parts.join('.');
}