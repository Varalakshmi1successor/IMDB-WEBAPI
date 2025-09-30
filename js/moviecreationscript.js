
  document.addEventListener("DOMContentLoaded", function () {
    const actorInput = document.getElementById("actorDropdown");
    const checkboxes = document.querySelectorAll(".actor-option");

    checkboxes.forEach(cb => {
      cb.addEventListener("change", function () {
        const selected = Array.from(checkboxes)
          .filter(i => i.checked)
          .map(i => i.value);
        actorInput.value = selected.join(", ");
        actorInput.setCustomValidity(selected.length > 0 ? "" : "Please select at least one actor.");
      });
    });

    const form = document.querySelector('.needs-validation');
    form.addEventListener("submit", function (event) {
      const selectedActors = actorInput.value.trim();
      actorInput.setCustomValidity(selectedActors ? "" : "Please select at least one actor.");
      if (!form.checkValidity()) {
        event.preventDefault();
        event.stopPropagation();
      }
      form.classList.add('was-validated');
    });
  });

  document.getElementById("poster").addEventListener("change", function () {
    const fileNameSpan = document.getElementById("fileName");
    fileNameSpan.textContent = this.files[0]?.name || "No file chosen";
  });
