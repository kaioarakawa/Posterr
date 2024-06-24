import "./PersonIcon.css";

const BORDER_TYPES = {
  rounded: "rounded-full",
  square: "rounded-md",
};

const SIZES = {
  sm: "h-8",
  md: "h-[64px]",
  xl: "h-32",
};

export default function PersonIcon({ size, border }) {
  const borderClass = BORDER_TYPES[border] || BORDER_TYPES.rounded;
  const sizeClass = SIZES[size] || SIZES.md;
  const className = `poster-person-icon ${sizeClass} ${borderClass}`;

  return (
    <img
      data-testid="person-icon"
      src="/user_default.png"
      alt="logged-user-avatar"
      className={className}
    />
  );
}
