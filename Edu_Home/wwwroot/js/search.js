var searchInput = document.getElementById("search-courses");
if (searchInput) {
    searchInput.addEventListener("keyup", function () {

        let text = this.value
        let courseList = document.querySelector("#course-list")

        fetch('Course/Search?searchText=' + text)
            .then((response) => response.text())
            .then((data) => {
                console.log(data)
                courseList.innerHTML = data
            });

    });
}