import Swal from "sweetalert2";

export const showSuccessMessage = (message) => {
  Toast.fire({
    icon: "success",
    title: message,
  });
};

export const showErrorMessage = (message) => {
  Toast.fire({
    icon: "error",
    title: message,
  });
};

export const repostMessage = (message) => {
  return Swal.fire({
    title: message,
    showCancelButton: true,
    confirmButtonText: "Repost",
    confirmButtonColor: "#2979FF",
    background: "#191919",
    color: "#6A7280",
  }).then((result) => {
    return result;
  });
};

export const repostWithQuoteMessage = (message) => {
  return Swal.fire({
    input: "text",
    inputLabel: message,
    inputPlaceholder: "Add a comment",
    showCancelButton: true,
    confirmButtonText: "Repost with quote",
    confirmButtonColor: "#2979FF",
    background: "#191919",
    color: "#6A7280",
  });
};

const Toast = Swal.mixin({
  toast: true,
  position: "top-end",
  showConfirmButton: false,
  timer: 3000,
  timerProgressBar: true,
  didOpen: (toast) => {
    toast.onmouseenter = Swal.stopTimer;
    toast.onmouseleave = Swal.resumeTimer;
  },
});
