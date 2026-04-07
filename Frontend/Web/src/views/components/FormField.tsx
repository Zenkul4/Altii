interface FormFieldProps {
  label:        string;
  name:         string;
  type?:        string;
  value:        string;
  onChange:     (e: React.ChangeEvent<HTMLInputElement>) => void;
  onBlur?:      (e: React.FocusEvent<HTMLInputElement>) => void;
  error?:       string;
  touched?:     boolean;
  placeholder?: string;
  required?:    boolean;
  disabled?:    boolean;
}

export default function FormField({
  label, name, type = "text", value, onChange, onBlur,
  error, touched, placeholder, required = false, disabled = false,
}: FormFieldProps) {
  const showError  = touched && error;
  const errorId    = `${name}-error`;
  const inputId    = `field-${name}`;

  return (
    <div>
      <label
        htmlFor={inputId}
        className="block text-[10px] tracking-[0.2em] uppercase text-muted mb-3"
      >
        {label}
        {required && <span aria-hidden="true" className="text-[#B8924A] ml-1">*</span>}
      </label>
      <input
        id={inputId}
        type={type}
        name={name}
        value={value}
        onChange={onChange}
        onBlur={onBlur}
        required={required}
        disabled={disabled}
        placeholder={placeholder}
        aria-invalid={showError ? "true" : "false"}
        aria-describedby={showError ? errorId : undefined}
        className="input-field"
        style={{ borderColor: showError ? "#e53e3e" : undefined }}
      />
      <div style={{ minHeight: "18px", marginTop: "4px" }}>
        {showError && (
          <p
            id={errorId}
            role="alert"
            className="text-red-500 text-[11px] tracking-wide"
          >
            {error}
          </p>
        )}
      </div>
    </div>
  );
}