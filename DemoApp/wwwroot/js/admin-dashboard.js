//Nút quay lại trang admin 
document.addEventListener('DOMContentLoaded', function () {
    const backToSiteBtn = document.querySelector('.back-to-site');

    if (backToSiteBtn) {
        backToSiteBtn.addEventListener('click', function (e) {
            console.log('Nút "Về trang chủ" được click');
            console.log('Href:', this.getAttribute('href'));

            // Nếu vẫn không hoạt động, thử dùng JavaScript redirect
            // e.preventDefault();
            // window.location.href = '/';
        });
    }
});
// Toggle sidebar functionality
document.addEventListener('DOMContentLoaded', function () {
    const sidebar = document.getElementById('adminSidebar');
    const mainContent = document.getElementById('adminMain');
    const sidebarToggle = document.getElementById('sidebarToggle');


    // Toggle sidebar
    sidebarToggle.addEventListener('click', function () {
        sidebar.classList.toggle('collapsed');
        mainContent.classList.toggle('expanded');

        // Update toggle button icon với hiệu ứng xoay
        const icon = sidebarToggle.querySelector('svg');
        if (sidebar.classList.contains('collapsed')) {
            // Khi thu gọn, đổi icon thành mũi tên sang phải
            icon.innerHTML = `
                <line x1="5" y1="12" x2="19" y2="12"></line>
                <polyline points="12 5 19 12 12 19"></polyline>
            `;
        } else {
            // Khi mở rộng, đổi icon thành mũi tên sang trái
            icon.innerHTML = `
                <line x1="19" y1="12" x2="5" y2="12"></line>
                <polyline points="12 19 5 12 12 5"></polyline>
            `;
        }
    });

    // Initialize revenue chart
    initializeRevenueChart();
});

// Revenue Chart
function initializeRevenueChart() {
    const ctx = document.getElementById('revenueChart').getContext('2d');

    const revenueData = {
        labels: ['Th1', 'Th2', 'Th3', 'Th4', 'Th5', 'Th6', 'Th7', 'Th8', 'Th9', 'Th10', 'Th11', 'Th12'],
        datasets: [{
            label: 'Doanh thu (triệu VNĐ)',
            data: [850, 920, 780, 1100, 1250, 980, 1350, 1420, 1280, 1560, 1480, 1720],
            backgroundColor: 'rgba(0, 80, 255, 0.1)',
            borderColor: '#0050FF',
            borderWidth: 2,
            fill: true,
            tension: 0.4
        }]
    };

    const revenueChart = new Chart(ctx, {
        type: 'line',
        data: revenueData,
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                },
                tooltip: {
                    mode: 'index',
                    intersect: false,
                    callbacks: {
                        label: function (context) {
                            return `Doanh thu: ${context.parsed.y.toLocaleString()} triệu VNĐ`;
                        }
                    }
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    grid: {
                        color: 'rgba(0, 0, 0, 0.05)'
                    },
                    ticks: {
                        callback: function (value) {
                            return value.toLocaleString() + ' tr';
                        }
                    }
                },
                x: {
                    grid: {
                        display: false
                    }
                }
            }
        }
    });
}