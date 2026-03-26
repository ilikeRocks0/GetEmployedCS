export function QuizContainer({ children }: { children?: React.ReactNode }){
    return (
        <div
        style={{
          width: 560,
          height: 400,
          background: "#fff",
          borderRadius: 20,
          boxShadow: "0 8px 40px rgba(0,0,0,0.12)",
          padding: 40,
          display: "flex",
          flexDirection: "column",
          justifyContent: "center",
          alignItems: "center",
          userSelect: "none",
          position: "relative",
          zIndex: 1, 
        }}
      >
        {children}
      </div>
    )
}