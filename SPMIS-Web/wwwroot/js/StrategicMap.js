document.addEventListener('DOMContentLoaded', function () {
    // Modal functionality
    const modal = document.getElementById('createMapModal');
    const openModalBtn = document.getElementById('openCreateMapModal');
    const closeModalBtn = document.getElementById('closeModal');
    const modalBody = document.getElementById('modalBody');

    // Open modal and load form
    openModalBtn.addEventListener('click', async () => {
        modal.classList.remove('hidden');
        try {
            const response = await fetch('/Map/CreateMap');
            const html = await response.text();
            modalBody.innerHTML = html;
        } catch (error) {
            console.error('Error loading modal content:', error);
            modalBody.innerHTML = '<div class="text-center text-red-500"><p>Error loading form. Please try again.</p></div>';
        }
    });

    // Close modal
    closeModalBtn.addEventListener('click', () => {
        modal.classList.add('hidden');
    });

    // Close modal on overlay click
    modal.addEventListener('click', (e) => {
        if (e.target === modal) {
            modal.classList.add('hidden');
        }
    });

    // Search and filter functionality
    const searchInput = document.getElementById('search-maps');
    const filterSelect = document.getElementById('filter-maps');
    const mapCards = document.querySelectorAll('.map-card');
    const mapList = document.getElementById('mapList');
    const noResults = document.getElementById('noResults');

    function filterMaps() {
        const searchTerm = searchInput.value.toLowerCase().trim();
        const filterValue = filterSelect.value;
        let visibleCount = 0;

        mapCards.forEach(card => {
            const title = card.querySelector('h3').textContent.toLowerCase();
            const description = card.querySelector('p:nth-child(2)').textContent.toLowerCase();
            const dateText = card.querySelector('p:nth-child(3)').textContent.toLowerCase();
            const status = card.dataset.status;

            // Check if card matches search term and filter
            const matchesSearch = searchTerm === '' ||
                title.includes(searchTerm) ||
                description.includes(searchTerm) ||
                dateText.includes(searchTerm);

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
        if (visibleCount === 0) {
            mapList.classList.add('hidden');
            noResults.classList.remove('hidden');
        } else {
            mapList.classList.remove('hidden');
            noResults.classList.add('hidden');
        }
    }

    // Add event listeners for search and filter
    searchInput.addEventListener('input', filterMaps);
    filterSelect.addEventListener('change', filterMaps);

    // Dark mode toggle functionality
    const darkModeToggle = document.getElementById('darkModeToggle');
    const moonIcon = darkModeToggle.querySelector('i');

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
            moonIcon.classList.remove('fa-moon');
            moonIcon.classList.add('fa-sun');
        } else {
            document.documentElement.classList.remove('dark');
            moonIcon.classList.remove('fa-sun');
            moonIcon.classList.add('fa-moon');
        }
    }

    // Initialize theme
    applyTheme(getThemePreference());

    // Toggle theme
    darkModeToggle.addEventListener('click', () => {
        const isDark = document.documentElement.classList.toggle('dark');
        localStorage.setItem('darkMode', isDark ? 'enabled' : 'disabled');

        if (isDark) {
            moonIcon.classList.remove('fa-moon');
            moonIcon.classList.add('fa-sun');
        } else {
            moonIcon.classList.remove('fa-sun');
            moonIcon.classList.add('fa-moon');
        }
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

            const startDate = new Date(document.getElementById('MapStart').value);
            const endDate = new Date(document.getElementById('MapEnd').value);
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
                    const result = await response.json();

                    if (result.exists) {
                        // Show validation error
                        errorDiv.textContent = "Only one active strategy map is allowed. Please adjust the dates.";
                        errorDiv.classList.remove('hidden');
                        return;
                    }
                } catch (error) {
                    console.error('Error checking for active maps:', error);
                }
            }

            // If everything is valid, submit the form
            this.submit();
        });
    }

    // Call this function when the modal is opened and the form is loaded
    if (openModalBtn) {
        const originalModalLoadHandler = openModalBtn.onclick;
        openModalBtn.onclick = async function () {
            // Call the original handler if it exists
            if (originalModalLoadHandler) {
                originalModalLoadHandler.call(this);
            }

            // Wait a bit for the form to load
            setTimeout(initCreateMapForm, 500);
        };
    }

    // Also initialize the form if it's already on the page (for edit forms)
    initCreateMapForm();

    // Add this to your existing JavaScript file
    document.addEventListener('DOMContentLoaded', function () {
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

                const startDate = new Date(document.getElementById('MapStart').value);
                const endDate = new Date(document.getElementById('MapEnd').value);
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
                        const result = await response.json();

                        if (result.exists) {
                            // Show validation error
                            errorDiv.textContent = "Only one active strategy map is allowed. Please adjust the dates.";
                            errorDiv.classList.remove('hidden');
                            return;
                        }
                    } catch (error) {
                        console.error('Error checking for active maps:', error);
                    }
                }

                // If everything is valid, submit the form
                this.submit();
            });
        }

        // Call this function when the modal is opened and the form is loaded
        const openModalBtn = document.getElementById('openCreateMapModal');
        if (openModalBtn) {
            const modalBody = document.getElementById('modalBody');

            const originalModalLoadHandler = openModalBtn.onclick;
            openModalBtn.onclick = async function () {
                // Call the original handler if it exists
                if (originalModalLoadHandler) {
                    originalModalLoadHandler.call(this);
                }

                // Wait a bit for the form to load
                setTimeout(initCreateMapForm, 500);
            };
        }

        // Also initialize the form if it's already on the page (for edit forms)
        initCreateMapForm();
    });
});
