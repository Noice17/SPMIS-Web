document.addEventListener('DOMContentLoaded', function () {
    // Handle objective card buttons
    document.querySelectorAll('.objective-card').forEach(card => {
        // Edit button
        card.querySelector('.edit-btn')?.addEventListener('click', function (event) {
            event.preventDefault();
            const objectiveId = card.getAttribute('data-id');
            const objectiveTitle = card.querySelector('h3').innerText;
            const objectiveType = card.getAttribute('data-type');

            // Open the edit modal
            openEditObjectiveModal(objectiveId, objectiveTitle, objectiveType);
        });

        // Assign button
        card.querySelector('.assign-btn')?.addEventListener('click', function (event) {
            event.preventDefault();
            const objectiveTitle = card.querySelector('h3').innerText;
            alert(`Assign clicked for: ${objectiveTitle}`);
            // Add your assign logic here
        });

        // View button
        card.querySelector('.view-btn')?.addEventListener('click', function (event) {
            event.preventDefault();
            const objectiveId = card.getAttribute('data-id');
            const objectiveTitle = card.querySelector('h3').innerText;
            const objectiveType = card.getAttribute('data-type');

            // Open the view modal
            openViewObjectiveModal(objectiveId, objectiveTitle, objectiveType);
        });
    });

    // Strategy Map Modal functionality
    const mapModal = document.getElementById('editMapModal');
    const editMapBtn = document.getElementById('editMapBtn');
    const closeMapModal = document.getElementById('closeModal');
    const cancelEditBtn = document.getElementById('cancelEditBtn');

    function openMapModal() {
        mapModal.classList.remove('hidden');
    }

    function closeMapModalFunc() {
        mapModal.classList.add('hidden');
    }

    if (editMapBtn) editMapBtn.addEventListener('click', openMapModal);
    if (closeMapModal) closeMapModal.addEventListener('click', closeMapModalFunc);
    if (cancelEditBtn) cancelEditBtn.addEventListener('click', closeMapModalFunc);

    // Close modal when clicking outside
    if (mapModal) {
        mapModal.addEventListener('click', function (event) {
            if (event.target === mapModal) {
                closeMapModalFunc();
            }
        });
    }

    // Form validation for start and end dates
    const editMapForm = document.getElementById('editMapForm');
    if (editMapForm) {
        const mapStartInput = document.getElementById('MapStart');
        const mapEndInput = document.getElementById('MapEnd');

        editMapForm.addEventListener('submit', function (event) {
            const startDate = new Date(mapStartInput.value);
            const endDate = new Date(mapEndInput.value);

            if (endDate <= startDate) {
                event.preventDefault();
                alert('End date must be after start date');
            }
        });
    }

    // View Objective Modal functionality
    function openViewObjectiveModal(objectiveId, objectiveTitle, objectiveType) {
        const viewModal = document.getElementById('viewObjectiveModal');
        const modalTitle = document.getElementById('viewObjectiveTitle');
        const modalType = document.getElementById('viewObjectiveType');
        const modalDescription = document.getElementById('viewObjectiveDescription');

        // Set modal content
        if (modalTitle) modalTitle.innerText = objectiveTitle;
        if (modalType) modalType.innerText = objectiveType;
        if (modalDescription) modalDescription.innerText = objectiveTitle;

        // Show modal
        if (viewModal) viewModal.classList.remove('hidden');
    }

    // Close View Objective Modal
    const closeViewBtn = document.getElementById('closeViewModal');
    if (closeViewBtn) {
        closeViewBtn.addEventListener('click', function () {
            document.getElementById('viewObjectiveModal').classList.add('hidden');
        });
    }

    // Edit Objective Modal functionality
    function openEditObjectiveModal(objectiveId, objectiveDescription, objectiveType) {
        const editModal = document.getElementById('editObjectiveModal');
        const objectiveDescInput = document.getElementById('editObjectiveDescription');
        const objectiveTypeSelect = document.getElementById('editObjectiveType');
        const objectiveIdInput = document.getElementById('editObjectiveId');

        // Set form values
        if (objectiveDescInput) objectiveDescInput.value = objectiveDescription;
        if (objectiveTypeSelect) {
            // Find and select the correct option
            Array.from(objectiveTypeSelect.options).forEach(option => {
                if (option.value === objectiveType) {
                    option.selected = true;
                }
            });
        }
        if (objectiveIdInput) objectiveIdInput.value = objectiveId;

        // Show modal
        if (editModal) editModal.classList.remove('hidden');
    }

    // Close Edit Objective Modal
    const closeEditObjectiveBtn = document.getElementById('closeEditObjective');
    const cancelEditObjectiveBtn = document.getElementById('cancelEditObjective');

    function closeEditObjectiveModal() {
        document.getElementById('editObjectiveModal').classList.add('hidden');
    }

    if (closeEditObjectiveBtn) closeEditObjectiveBtn.addEventListener('click', closeEditObjectiveModal);
    if (cancelEditObjectiveBtn) cancelEditObjectiveBtn.addEventListener('click', closeEditObjectiveModal);

    // Close modals when clicking outside
    document.getElementById('viewObjectiveModal')?.addEventListener('click', function (event) {
        if (event.target === this) {
            this.classList.add('hidden');
        }
    });

    document.getElementById('editObjectiveModal')?.addEventListener('click', function (event) {
        if (event.target === this) {
            this.classList.add('hidden');
        }
    });

    // Form validation for edit objective form
    const editObjectiveForm = document.getElementById('editObjectiveForm');
    if (editObjectiveForm) {
        editObjectiveForm.addEventListener('submit', function (event) {
            const description = document.getElementById('editObjectiveDescription').value;
            if (!description.trim()) {
                event.preventDefault();
                alert('Objective description cannot be empty');
            }
        });
    }

    // Dynamic Category Counting & Display
    function updateCategoryStats() {
        const objectiveCards = document.querySelectorAll('.objective-card:not(.hidden)');
        const categoryData = {};

        // Count visible objectives by category
        objectiveCards.forEach(card => {
            const type = card.getAttribute('data-type');
            if (!categoryData[type]) {
                categoryData[type] = 0;
            }
            categoryData[type]++;
        });

        // Update total count
        const totalCountElement = document.querySelector('.container.mx-auto.max-w-6xl .flex.flex-wrap .text-xl.font-bold');
        if (totalCountElement) {
            totalCountElement.textContent = objectiveCards.length;
        }

        // Find the stats container
        const categoryStatsContainer = document.querySelector('.flex.flex-wrap.justify-center.items-center.text-center.gap-20');
        if (!categoryStatsContainer) return;

        // Keep only the first child (total count) and remove the rest
        const totalStatsDiv = categoryStatsContainer.firstElementChild;
        while (categoryStatsContainer.childNodes.length > 1) {
            categoryStatsContainer.removeChild(categoryStatsContainer.lastChild);
        }

        // Add back the first child (total count)
        if (totalStatsDiv && !categoryStatsContainer.contains(totalStatsDiv)) {
            categoryStatsContainer.appendChild(totalStatsDiv);
        }

        // Add category counters
        Object.entries(categoryData).forEach(([category, count]) => {
            // Skip empty categories
            if (!category || category === "N/A") return;

            // Determine icon and color based on category
            let iconClass = "fas fa-tag";
            let colorClass = "text-green-600 dark:text-green-400";

            if (category === "Core Strategy") {
                iconClass = "fas fa-bullseye";
                colorClass = "text-blue-600 dark:text-blue-400";
            } else if (category === "Support Strategy") {
                iconClass = "fas fa-cogs";
                colorClass = "text-purple-600 dark:text-purple-400";
            }

            const categoryDiv = document.createElement('div');
            categoryDiv.className = "flex items-center space-x-2 px-6 py-4 border-r border-gray-200 dark:border-gray-800";
            categoryDiv.innerHTML = `
                <i class="${iconClass} ${colorClass} text-2xl"></i>
                <div>
                    <p class="text-xl font-bold text-gray-800 dark:text-white">${count}</p>
                    <p class="text-xs text-gray-500 dark:text-gray-400 uppercase tracking-wider">${category}</p>
                </div>
            `;

            categoryStatsContainer.appendChild(categoryDiv);
        });
    }

    // Search and filter functionality
    const searchInput = document.getElementById('searchObjective');
    const typeFilter = document.getElementById('filterObjectiveType');
    const objectiveCards = document.querySelectorAll('.objective-card');
    const noResults = document.getElementById('noResults');
    const objectivesGrid = document.getElementById('objectivesGrid');

    function filterObjectives() {
        const searchTerm = searchInput?.value.toLowerCase() || '';
        const selectedType = typeFilter?.value || '';
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
        if (noResults && objectivesGrid) {
            if (visibleCount === 0) {
                noResults.classList.remove('hidden');
                objectivesGrid.classList.add('hidden');
            } else {
                noResults.classList.add('hidden');
                objectivesGrid.classList.remove('hidden');
            }
        }

        // Update category stats after filtering
        updateCategoryStats();
    }

    // Reset search and filters
    const resetSearchBtn = document.getElementById('resetSearch');
    if (resetSearchBtn) {
        resetSearchBtn.addEventListener('click', function () {
            if (searchInput) searchInput.value = '';
            if (typeFilter) typeFilter.value = '';
            filterObjectives();
        });
    }

    // Add event listeners for search and filter
    if (searchInput) searchInput.addEventListener('input', filterObjectives);
    if (typeFilter) typeFilter.addEventListener('change', filterObjectives);

    // Initialize category counts on page load
    updateCategoryStats();
});