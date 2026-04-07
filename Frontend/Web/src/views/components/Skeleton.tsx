interface SkeletonProps { className?:string; width?:string; height?:string; }

export function Skeleton({ className="", width="100%", height="16px" }: SkeletonProps) {
  return (
    <div className={className} style={{
      width, height,
      background:"linear-gradient(90deg, var(--bg-200) 25%, var(--bg-300) 50%, var(--bg-200) 75%)",
      backgroundSize:"200% 100%",
      animation:"shimmer 1.5s infinite",
      borderRadius:"2px",
    }} />
  );
}

export function RoomCardSkeleton() {
  return (
    <div className="bg-bg-white border border-bg-card overflow-hidden">
      <Skeleton height="224px" />
      <div className="p-6 space-y-3">
        <div className="flex justify-between">
          <Skeleton width="40%" height="24px" />
          <Skeleton width="20%" height="24px" />
        </div>
        <Skeleton width="60%" height="14px" />
        <Skeleton width="80%" height="14px" />
        <Skeleton height="44px" className="mt-4" />
      </div>
    </div>
  );
}

export function BookingCardSkeleton() {
  return (
    <div className="bg-bg-white border border-bg-card p-6 md:p-8">
      <div className="flex items-center gap-3 mb-4">
        <Skeleton width="24px"  height="24px" />
        <Skeleton width="160px" height="28px" />
        <Skeleton width="80px"  height="20px" />
      </div>
      <div className="grid grid-cols-2 md:grid-cols-4 gap-6">
        {[1,2,3,4].map(i => (
          <div key={i} className="space-y-2">
            <Skeleton width="60%" height="12px" />
            <Skeleton height="18px" />
          </div>
        ))}
      </div>
    </div>
  );
}

export function BookingDetailSkeleton() {
  return (
    <div className="max-w-4xl mx-auto px-6 py-12 space-y-6">
      <Skeleton width="120px" height="14px" />
      <Skeleton height="120px" />
      <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
        <div className="bg-bg-white border border-bg-card p-8 space-y-4">
          <Skeleton width="40%" height="24px" />
          {[1,2,3,4,5].map(i => (
            <div key={i} className="flex justify-between pb-3 border-b border-bg-card">
              <Skeleton width="35%" height="14px" />
              <Skeleton width="25%" height="14px" />
            </div>
          ))}
        </div>
        <div className="bg-bg-white border border-bg-card p-8 space-y-4">
          <Skeleton width="30%" height="24px" />
          <Skeleton height="80px" />
        </div>
      </div>
    </div>
  );
}

export function ServicesSkeleton() {
  return (
    <div className="max-w-7xl mx-auto px-6 py-16">
      <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
        {[1,2,3,4,5,6].map(i => (
          <div key={i} className="bg-bg-white border border-bg-card p-8 space-y-3">
            <Skeleton width="32px" height="32px" />
            <Skeleton width="60%" height="22px" />
            <Skeleton height="14px" />
            <Skeleton width="80%" height="14px" />
          </div>
        ))}
      </div>
    </div>
  );
}