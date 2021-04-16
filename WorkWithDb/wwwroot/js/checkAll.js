$("#checkAll").change(function () {
    $('input[id="state"]').prop('checked', this.checked);
});