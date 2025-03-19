    document.addEventListener('DOMContentLoaded', function () {
    // Modal functionality
    const modal = document.getElementById('createMapModal');
    const openModalBtn = document.getElementById('openCreateMapModal');
    const openEmptyModalBtn = document.getElementById('openCreateMapModalEmpty');
    const closeModalBtn = document.getElementById('closeModal');
    const modalBody = document.getElementById('modalBody');
    const resetSearchBtn = document.getElementById('resetSearch');

    // Function to open modal and load form
    async function openModal() {
        modal.classList.remove('hidden');
        try {
            const response = await fetch('/Map/CreateMap');
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const html = await response.text();
            modalBody.innerHTML = html;
            initCreateMapForm(); // Initialize form validation after loading the form
        } catch (error) {
            console.error('Error loading modal content:', error);
            modalBody.innerHTML = '<div class="text-center text-red-500 py-4"><p>Error loading form. Please try again.</p><button class="mt-3 bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700" onclick="location.reload()">Reload Page</button></div>';
        }
    }

    // Open modal event listeners
    openModalBtn?.addEventListener('click', openModal);
    openEmptyModalBtn?.addEventListener('click', openModal);

    // Close modal
    closeModalBtn?.addEventListener('click', () => {
        modal.classList.add('hidden');
    });

    // Close modal on overlay click
    modal?.addEventListener('click', (e) => {
        if (e.target === modal) {
            modal.classList.add('hidden');
        }
    });

    // Search and filter functionality
    const searchInput = document.getElementById('search-maps');
    const filterSelect = document.getElementById('filter-maps');
    const mapCards = document.querySelectorAll('.map-card');
    const mapListSection = document.querySelector('.grid.grid-cols-1.md\\:grid-cols-2.lg\\:grid-cols-3.gap-6');
    const noResults = document.getElementById('noResults');

    function filterMaps() {
        const searchTerm = searchInput?.value.toLowerCase().trim() || '';
        const filterValue = filterSelect?.value || 'all';
        let visibleCount = 0;

        mapCards.forEach(card => {
            const title = card.querySelector('h3')?.textContent.toLowerCase() || '';
            const description = card.querySelector('p')?.textContent.toLowerCase() || '';
            const dateSpan = card.querySelector('.text-gray-600.dark\\:text-gray-400.text-sm')?.textContent.toLowerCase() || '';
            const status = card.dataset.status || '';

            // Check if card matches search term and filter
            const matchesSearch = searchTerm === '' ||
                title.includes(searchTerm) ||
                description.includes(searchTerm) ||
                dateSpan.includes(searchTerm);

            const matchesFilter = filterValue === 'all' || status === filterValue;

            // Show/hide the card based on both conditions
            if (matchesSearch && matchesFilter) {
                card.classList.remove('hidden');
                visibleCount++;
            } else {
                card.classList.add('hidden');
            }
        });

        // Show/hide no results message
        if (visibleCount === 0 && mapCards.length > 0) {
            mapListSection?.classList.add('hidden');
            noResults?.classList.remove('hidden');
        } else {
            mapListSection?.classList.remove('hidden');
            noResults?.classList.add('hidden');
        }
    }

    // Add event listeners for search and filter
    searchInput?.addEventListener('input', filterMaps);
    filterSelect?.addEventListener('change', filterMaps);

    // Reset search button
    resetSearchBtn?.addEventListener('click', () => {
        if (searchInput) searchInput.value = '';
        if (filterSelect) filterSelect.value = 'all';
        filterMaps();
    });

    // Dark mode toggle functionality
    const darkModeToggle = document.getElementById('darkModeToggle');

    // Check for saved theme preference or use system preference
    function getThemePreference() {
        if (localStorage.getItem('darkMode') === 'enabled') {
            return true;
        }
        if (localStorage.getItem('darkMode') === 'disabled') {
            return false;
        }
        return window.matchMedia('(prefers-color-scheme: dark)').matches;
    }

    // Apply theme
    function applyTheme(isDark) {
        if (isDark) {
            document.documentElement.classList.add('dark');
            if (darkModeToggle) {
                const moonIcon = darkModeToggle.querySelector('.fa-moon');
                const sunIcon = darkModeToggle.querySelector('.fa-sun');
                if (moonIcon) moonIcon.classList.add('hidden');
                if (sunIcon) sunIcon.classList.remove('hidden');
            }
        } else {
            document.documentElement.classList.remove('dark');
            if (darkModeToggle) {
                const moonIcon = darkModeToggle.querySelector('.fa-moon');
                const sunIcon = darkModeToggle.querySelector('.fa-sun');
                if (moonIcon) moonIcon.classList.remove('hidden');
                if (sunIcon) sunIcon.classList.add('hidden');
            }
        }
    }

    // Initialize theme
    applyTheme(getThemePreference());

    // Toggle theme
    darkModeToggle?.addEventListener('click', () => {
        const isDark = document.documentElement.classList.toggle('dark');
        localStorage.setItem('darkMode', isDark ? 'enabled' : 'disabled');
        applyTheme(isDark);
    });

    // Function to initialize form validation after the form is loaded
    function initCreateMapForm() {
        const form = document.querySelector('#createMapForm');
        if (!form) return;

        // Get the validation error div or create one if it doesn't exist
        let errorDiv = document.getElementById('validation-error');
        if (!errorDiv) {
            errorDiv = document.createElement('div');
            errorDiv.id = 'validation-error';
            errorDiv.className = 'hidden text-red-500 text-center mb-4';
            form.prepend(errorDiv);
        }

        form.addEventListener('submit', async function (e) {
            e.preventDefault();

            // Clear previous errors
            errorDiv.textContent = '';
            errorDiv.classList.add('hidden');

            const titleInput = document.getElementById('MapTitle');
            const startDateInput = document.getElementById('MapStart');
            const endDateInput = document.getElementById('MapEnd');

            // Check if required fields exist and are filled
            if (!titleInput || !titleInput.value.trim()) {
                errorDiv.textContent = "Title is required.";
                errorDiv.classList.remove('hidden');
                return;
            }

            if (!startDateInput || !startDateInput.value) {
                errorDiv.textContent = "Start date is required.";
                errorDiv.classList.remove('hidden');
                return;
            }

            if (!endDateInput || !endDateInput.value) {
                errorDiv.textContent = "End date is required.";
                errorDiv.classList.remove('hidden');
                return;
            }

            const startDate = new Date(startDateInput.value);
            const endDate = new Date(endDateInput.value);
            const now = new Date();

            // Basic validation
            if (startDate >= endDate) {
                errorDiv.textContent = "Start date must be before end date.";
                errorDiv.classList.remove('hidden');
                return;
            }

            // Check if this would be an active map
            const wouldBeActive = startDate <= now && endDate > now;

            if (wouldBeActive) {
                // Get the map ID if we're editing
                const mapIdField = document.getElementById('MapId');
                const mapId = mapIdField ? mapIdField.value : null;

                // Make an AJAX call to check if an active map exists
                try {
                    const url = mapId
                        ? `/Map/CheckActiveMapExists?excludeId=${mapId}`
                        : '/Map/CheckActiveMapExists';

                    const response = await fetch(url);
                    if (!response.ok) {
                        throw new Error(`HTTP error! Status: ${response.status}`);
                    }

                    const result = await response.json();

                    if (result.exists) {
                        // Show validation error
                        errorDiv.textContent = "Only one active strategy map is allowed. Please adjust the dates.";
                        errorDiv.classList.remove('hidden');
                        return;
                    }
                } catch (error) {
                    console.error('Error checking for active maps:', error);
                    errorDiv.textContent = "Error validating dates. Please try again.";
                    errorDiv.classList.remove('hidden');
                    return;
                }
            }

            // If everything is valid, submit the form
            try {
                this.submit();
            } catch (error) {
                console.error('Form submission error:', error);
                errorDiv.textContent = "Error submitting form. Please try again.";
                errorDiv.classList.remove('hidden');
            }
        });
    }

    // Initialize the form if it's already on the page (for edit forms)
    initCreateMapForm();
});