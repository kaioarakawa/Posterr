import "./Button.css";

export default function Button({ text, action, disabled }) {
  return (
    <button
      data-testid="button"
      className="poster-button"
      onClick={action}
      disabled={disabled}
    >
      <span>{text}</span>
    </button>
  );
}
