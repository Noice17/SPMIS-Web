
    document.addEventListener('DOMContentLoaded', function () {
        // Handle grid buttons
        document.querySelector('.grid')?.addEventListener('click', function (event) {
            const button = event.target.closest('button');
            if (!button) return;
            alert(button.innerText + " clicked!");
        });

        // Modal functionality
        const modal = document.getElementById('editMapModal');
        const editMapBtn = document.getElementById('editMapBtn');
        const closeModal = document.getElementById('closeModal');
        const cancelEditBtn = document.getElementById('cancelEditBtn');

        function openModal() {
            modal.classList.remove('hidden');
        }

        function closeModalFunc() {
            modal.classList.add('hidden');
        }

        editMapBtn.addEventListener('click', openModal);
        closeModal.addEventListener('click', closeModalFunc);
        cancelEditBtn.addEventListener('click', closeModalFunc);

        // Close modal when clicking outside
        modal.addEventListener('click', function(event) {
            if (event.target === modal) {
                closeModalFunc();
            }
        });

        // Form validation for start and end dates
        const mapStartInput = document.getElementById('MapStart');
        const mapEndInput = document.getElementById('MapEnd');

        document.getElementById('editMapForm').addEventListener('submit', function(event) {
            const startDate = new Date(mapStartInput.value);
            const endDate = new Date(mapEndInput.value);

            if (endDate <= startDate) {
                event.preventDefault();
                alert('End date must be after start date');
            }
        });

        // Search and filter functionality
        const searchInput = document.getElementById('searchObjective');
        const typeFilter = document.getElementById('filterObjectiveType');
        const objectiveCards = document.querySelectorAll('.objective-card');
        const noResults = document.getElementById('noResults');
        const objectivesGrid = document.getElementById('objectivesGrid');

        function filterObjectives() {
            const searchTerm = searchInput.value.toLowerCase();
            const selectedType = typeFilter.value;
            let visibleCount = 0;

            objectiveCards.forEach(card => {
                const description = card.getAttribute('data-description').toLowerCase();
                const type = card.getAttribute('data-type');
                const matchesSearch = description.includes(searchTerm);
                const matchesType = selectedType === '' || type === selectedType;

                if (matchesSearch && matchesType) {
                    card.classList.remove('hidden');
                    visibleCount++;
                } else {
                    card.classList.add('hidden');
                }
            });

            // Show/hide no results message
            if (visibleCount === 0) {
                noResults.classList.remove('hidden');
                objectivesGrid.classList.add('hidden');
            } else {
                noResults.classList.add('hidden');
                objectivesGrid.classList.remove('hidden');
            }
        }

        // Add event listeners for search and filter
        searchInput.addEventListener('input', filterObjectives);
        typeFilter.addEventListener('change', filterObjectives);
    });